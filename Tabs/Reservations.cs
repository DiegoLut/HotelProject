using HotelRoomsManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelRoomsManagementSystem.Tabs
{
    public class Reservations
    {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        DataRow lastRecord;
        public Reservations(DatabaseHelper dbHelper)
        {
            databaseHelper = dbHelper;
        }

        public void SaveReservationsChanges()
        {
            try
            {
                DataSet dsAdded = databaseHelper.dataSet.GetChanges(DataRowState.Added);
                if (dsAdded != null)
                {
                    foreach (DataRow row in dsAdded.Tables["Rezerwacja"].Rows)
                    {
                        string error;
                        if (!ValidateReservationRow(row, out error))
                        {
                            MessageBox.Show("Błąd walidacji: " + error, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        conn.Open();

                        var insertCmd = new OleDbCommand("INSERT INTO Rezerwacja (NumerPokoju, Email, DataZameldowania, DataWymeldowania, Rabat) VALUES (?, ?, ?, ?, ?)", conn);
                        
                        insertCmd.Parameters.Add("NumerPokoju", OleDbType.VarChar, 10, "NumerPokoju");
                        insertCmd.Parameters.Add("Email", OleDbType.VarChar, 10, "Email");
                        insertCmd.Parameters.Add("DataZameldowania", OleDbType.Date, 10, "DataZameldowania");
                        insertCmd.Parameters.Add("DataWymeldowania", OleDbType.Date, 50, "DataWymeldowania");
                        insertCmd.Parameters.Add("Rabat", OleDbType.Decimal, 0, "Rabat");

                        databaseHelper.adapterReservations.InsertCommand = insertCmd;
                        databaseHelper.adapterReservations.Update(dsAdded, "Rezerwacja");

                        CalculatePrice();

                        var idCmd = new OleDbCommand("SELECT @@IDENTITY", conn);
                        int newReservationId = Convert.ToInt32(idCmd.ExecuteScalar());
                        int cenaLaczna = Convert.ToInt32(lastRecord["CenaLaczna"]);
                        var nazwaUslugi = Convert.ToString(lastRecord["NazwaUslugi"]);
                        // 4. INSERT do RezerwacjaUsluga
                        var insertCmd2 = new OleDbCommand(
                            "INSERT INTO RezerwacjaUsluga (RezerwacjaID, NazwaUslugi, CenaLaczna) VALUES (?, ?, ?)", conn);

                        insertCmd2.Parameters.AddWithValue("RezerwacjaID", newReservationId);
                        insertCmd2.Parameters.AddWithValue("NazwaUslugi", nazwaUslugi);
                        insertCmd2.Parameters.AddWithValue("CenaLaczna", cenaLaczna);

                        //databaseHelper.adapterReservationsWithServices.InsertCommand = insertCmd2;
                        //databaseHelper.adapterReservationsWithServices.Update(dsAdded, "RezerwacjaUsluga");
                        insertCmd2.ExecuteNonQuery();


                        conn.Close();

                    }
                    MessageBox.Show("Nowe rezerwacje zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu rezerwacji: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void SaveUpdatedReservations()
        {
            try
            {
                DataSet dsModified = databaseHelper.dataSet.GetChanges(DataRowState.Modified);
                if (dsModified != null)
                {
                    foreach (DataRow row in dsModified.Tables["Rezerwacja"].Rows)
                    {
                        string error;
                        if (!ValidateReservationRow(row, out error))
                        {
                            MessageBox.Show("Błąd walidacji: " + error, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        conn.Open();

                        var updateCmd = new OleDbCommand(
                            "UPDATE Rezerwacja SET NumerPokoju = ?, Email = ?, DataZameldowania = ?, DataWymeldowania = ?, Rabat = ? WHERE RezerwacjaID = ?", conn);

                        updateCmd.Parameters.Add("NumerPokoju", OleDbType.VarChar, 10, "NumerPokoju");
                        updateCmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                        updateCmd.Parameters.Add("DataZameldowania", OleDbType.Date, 0, "DataZameldowania");
                        updateCmd.Parameters.Add("DataWymeldowania", OleDbType.Date, 0, "DataWymeldowania");
                        updateCmd.Parameters.Add("Rabat", OleDbType.Decimal, 0, "Rabat");
                        updateCmd.Parameters.Add("RezerwacjaID", OleDbType.Integer, 0, "RezerwacjaID").SourceVersion = DataRowVersion.Original;

                        databaseHelper.adapterReservations.UpdateCommand = updateCmd;
                        databaseHelper.adapterReservations.Update(dsModified, "Rezerwacja");

                        CalculatePrice();

                        if (lastRecord != null && lastRecord["RezerwacjaID"] != DBNull.Value)
                        {
                            int reservationId = Convert.ToInt32(lastRecord["RezerwacjaID"]);
                            int cenaLaczna = Convert.ToInt32(lastRecord["CenaLaczna"]);
                            string nazwaUslugi = Convert.ToString(lastRecord["NazwaUslugi"]);

                            var updateCmd2 = new OleDbCommand(
                                "UPDATE RezerwacjaUsluga SET NazwaUslugi = ?, CenaLaczna = ? WHERE RezerwacjaID = ?", conn);

                            updateCmd2.Parameters.AddWithValue("NazwaUslugi", nazwaUslugi);
                            updateCmd2.Parameters.AddWithValue("CenaLaczna", cenaLaczna);
                            updateCmd2.Parameters.AddWithValue("RezerwacjaID", reservationId);

                            updateCmd2.ExecuteNonQuery();
                        }

                        conn.Close();
                    }

                    MessageBox.Show("Zmodyfikowane rezerwacje zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmodyfikowanych rezerwacji: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CalculatePrice()
        {
            var ds = databaseHelper.dataSet;
            DataTable rezerwacjaTable = ds.Tables["Rezerwacja"];

            if (rezerwacjaTable.Rows.Count > 0)
            {
                DataTable uslugi = ds.Tables["Usluga"];
                DataTable pokoje = ds.Tables["Pokoj"];

                var lastIndex = rezerwacjaTable.Rows.Count - 1;
                lastRecord = rezerwacjaTable.Rows[lastIndex];

                DateTime zameldowanie = Convert.ToDateTime(lastRecord["DataZameldowania"]);
                DateTime wymeldowanie = Convert.ToDateTime(lastRecord["DataWymeldowania"]);

                int liczbaDni = (wymeldowanie - zameldowanie).Days;

                if (liczbaDni < 0)
                {
                    MessageBox.Show("Data wymeldowania nie może być wcześniejsza niż zameldowania. ", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int cenaZaUsluge = 0;
                int cenaZaNoc = 0;


                if(lastRecord["NumerPokoju"] != DBNull.Value)
                {
                    string numerPokoju = lastRecord["NumerPokoju"].ToString();


                    DataRow[] znalezionePokoje = pokoje.Select($"NumerPokoju = '{numerPokoju.Replace("'", "''")}'");
                    if (znalezionePokoje.Length > 0)
                        cenaZaNoc = Convert.ToInt32(znalezionePokoje[0]["CenaZaNoc"]);
                }


                int rabat = 0;
                int cenaLaczna = 0;

                if (lastRecord["Rabat"] != DBNull.Value)
                {
                    rabat = Convert.ToInt32(lastRecord["Rabat"]);
                }

                if (lastRecord["NazwaUslugi"] != DBNull.Value)
                {
                    string nazwaUslugi = lastRecord["NazwaUslugi"].ToString();


                    DataRow[] znalezioneUslugi = uslugi.Select($"Nazwa = '{nazwaUslugi.Replace("'", "''")}'");

                    if (znalezioneUslugi.Length > 0)
                        cenaZaUsluge = Convert.ToInt32(znalezioneUslugi[0]["Cena"]);
                }

                cenaLaczna = liczbaDni * cenaZaNoc + cenaZaUsluge - rabat;

                lastRecord["CenaLaczna"] = cenaLaczna;
            }
        }

        private bool ValidateReservationRow(DataRow row, out string errorMessage)
        {
            errorMessage = "";

            var ds = databaseHelper.dataSet;

            string email = row["Email"]?.ToString();
            if (string.IsNullOrWhiteSpace(email))
            {
                errorMessage = "Email nie może być pusty.";
                return false;
            }

            bool emailExists = ds.Tables["Klient"].Select($"Email = '{email.Replace("'", "''")}'").Length > 0;
            if (!emailExists)
            {
                errorMessage = $"Email '{email}' nie istnieje w bazie klientów.";
                return false;
            }

            if (row["NazwaUslugi"] == DBNull.Value || string.IsNullOrWhiteSpace(row["NazwaUslugi"].ToString()))
            {
                errorMessage = "Brak wybranej usługi.";
                return false;
            }

            string usluga = row["NazwaUslugi"].ToString();
            bool uslugaExists = ds.Tables["Usluga"].Select($"Nazwa = '{usluga.Replace("'", "''")}'").Length > 0;
            if (!uslugaExists)
            {
                errorMessage = $"Usługa '{usluga}' nie istnieje w bazie.";
                return false;
            }

            if (row["NumerPokoju"] == DBNull.Value || string.IsNullOrWhiteSpace(row["NumerPokoju"].ToString()))
            {
                errorMessage = "Brak numeru pokoju.";
                return false;
            }

            string room = row["NumerPokoju"].ToString();
            bool roomExists = ds.Tables["Pokoj"].Select($"NumerPokoju = '{room.Replace("'", "''")}'").Length > 0;
            if (!roomExists)
            {
                errorMessage = $"Pokój o numerze '{room}' nie istnieje w bazie.";
                return false;
            }

            DateTime start = Convert.ToDateTime(row["DataZameldowania"]);
            DateTime end = Convert.ToDateTime(row["DataWymeldowania"]);
            string room1 = row["NumerPokoju"]?.ToString();

            if (end < start)
            {
                errorMessage = "Data wymeldowania nie może być wcześniejsza niż zameldowania.";
                return false;
            }

            var allReservations = ds.Tables["Rezerwacja"].Rows;
            foreach (DataRow existingRow in allReservations)
            {
                if (existingRow.RowState == DataRowState.Deleted) continue;

                if (row != existingRow && existingRow["NumerPokoju"].ToString() == room1)
                {
                    DateTime existingStart = Convert.ToDateTime(existingRow["DataZameldowania"]);
                    DateTime existingEnd = Convert.ToDateTime(existingRow["DataWymeldowania"]);

                    bool overlaps = start < existingEnd && end > existingStart;
                    if (overlaps)
                    {
                        errorMessage = $"Rezerwacja dla pokoju {room1} koliduje z inną rezerwacją w terminie {existingStart.ToShortDateString()} - {existingEnd.ToShortDateString()}";
                        return false;
                    }
                }
            }

            return true;
        }

    }




}
