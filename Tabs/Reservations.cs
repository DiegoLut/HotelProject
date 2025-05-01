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
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var insertCmd = new OleDbCommand("INSERT INTO Rezerwacja (NumerPokoju, Email, DataZameldowania, DataWymeldowania, Rabat) VALUES (?, ?, ?, ?, ?)", conn);
                        
                        insertCmd.Parameters.Add("NumerPokoju", OleDbType.VarChar, 10, "NumerPokoju");
                        insertCmd.Parameters.Add("Email", OleDbType.VarChar, 10, "Email");
                        insertCmd.Parameters.Add("DataZameldowania", OleDbType.Date, 10, "DataZameldowania");
                        insertCmd.Parameters.Add("DataWymeldowania", OleDbType.Date, 50, "DataWymeldowania");
                        insertCmd.Parameters.Add("Rabat", OleDbType.Decimal, 0, "Rabat");

                        databaseHelper.adapterReservations.InsertCommand = insertCmd;
                        databaseHelper.adapterReservations.Update(dsAdded, "Rezerwacja");

                        CalculatePrice();

                        int cenaLaczna = Convert.ToInt32(lastRecord["CenaLaczna"]);

                        // 4. INSERT do RezerwacjaUsluga
                        var insertCmd2 = new OleDbCommand(
                            "INSERT INTO RezerwacjaUsluga (RezerwacjaID, NazwaUslugi, CenaLaczna) VALUES (?, ?, ?)", conn);

                        //insertCmd2.Parameters.AddWithValue("RezerwacjaID", "RezerwacjaID");
                        //insertCmd2.Parameters.Add("NazwaUslugi", "NazwaUslugi");
                        //insertCmd2.Parameters.AddWithValue("CenaLaczna", cenaLaczna);

                        //insertCmd2.ExecuteNonQuery();


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
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var updateCmd = new OleDbCommand("UPDATE Rezerwacja SET DataZameldowania = ?, DataWymeldowania = ?, Rabat = ? WHERE RezerwacjaID = ?", conn);
                        updateCmd.Parameters.Add("DataZameldowania", OleDbType.VarChar, 10, "DataZameldowania");
                        updateCmd.Parameters.Add("DataWymeldowania", OleDbType.VarChar, 50, "DataWymeldowania");
                        updateCmd.Parameters.Add("Rabat", OleDbType.Boolean, 0, "Rabat");

                        CalculatePrice();

                        databaseHelper.adapterReservations.UpdateCommand = updateCmd;
                        databaseHelper.adapterRooms.Update(dsModified, "Rezerwacja");
                    }
                    MessageBox.Show("Zmodyfikowane rezerwacje zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmian rezerwacji: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Przykładowe dane do obliczeń
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

        public void Get()
        {

        }
    }




}
