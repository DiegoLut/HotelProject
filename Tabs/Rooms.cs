using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using HotelRoomsManagementSystem.Model;

namespace HotelRoomsManagementSystem.Tabs
{
    public class Rooms
    {
        private DatabaseHelper databaseHelper;

        public Rooms(DatabaseHelper dbHelper)
        {
            databaseHelper = dbHelper;
        }

        public void SaveInsertedRooms()
        {
            try
            {
                DataSet dsAdded = databaseHelper.dataSet.GetChanges(DataRowState.Added);
                if (dsAdded != null && dsAdded.Tables["Pokoj"] != null)
                {
                    foreach (DataRow row in dsAdded.Tables["Pokoj"].Rows)
                    {
                        if (row.IsNull("Dostepnosc"))
                        {
                            row["Dostepnosc"] = false;
                        }
                        if (!ValidateRoomRow(row, out string errorMessage))
                        {
                            MessageBox.Show(errorMessage, "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var insertCmd = new OleDbCommand("INSERT INTO Pokoj (NumerPokoju, TypPokoju, CenaZaNoc, Dostepnosc) VALUES (?, ?, ?, ?)", conn);
                        insertCmd.Parameters.Add("NumerPokoju", OleDbType.VarChar, 10, "NumerPokoju");
                        insertCmd.Parameters.Add("TypPokoju", OleDbType.VarChar, 50, "TypPokoju");
                        insertCmd.Parameters.Add("CenaZaNoc", OleDbType.Currency, 0, "CenaZaNoc");
                        insertCmd.Parameters.Add("Dostepnosc", OleDbType.Boolean, 0, "Dostepnosc");

                        databaseHelper.adapterRooms.InsertCommand = insertCmd;
                        databaseHelper.adapterRooms.Update(dsAdded, "Pokoj");
                    }
                    MessageBox.Show("Nowe pokoje zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu pokoi: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveUpdatedRooms()
        {
            try
            {
                DataSet dsModified = databaseHelper.dataSet.GetChanges(DataRowState.Modified);
                if (dsModified != null && dsModified.Tables["Pokoj"] != null)
                {
                    foreach (DataRow row in dsModified.Tables["Pokoj"].Rows)
                    {
                        if (row.IsNull("Dostepnosc"))
                        {
                            row["Dostepnosc"] = false;
                        }
                        if (!ValidateRoomRow(row, out string errorMessage))
                        {
                            MessageBox.Show(errorMessage, "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var updateCmd = new OleDbCommand("UPDATE Pokoj SET NumerPokoju = ?, TypPokoju = ?, CenaZaNoc = ?, Dostepnosc = ? WHERE PokojID = ?", conn);
                        updateCmd.Parameters.Add("NumerPokoju", OleDbType.VarChar, 10, "NumerPokoju");
                        updateCmd.Parameters.Add("TypPokoju", OleDbType.VarChar, 50, "TypPokoju");
                        updateCmd.Parameters.Add("CenaZaNoc", OleDbType.Currency, 0, "CenaZaNoc");
                        updateCmd.Parameters.Add("Dostepnosc", OleDbType.Boolean, 0, "Dostepnosc");
                        updateCmd.Parameters.Add("PokojID", OleDbType.Integer, 0, "PokojID").SourceVersion = DataRowVersion.Original;

                        databaseHelper.adapterRooms.UpdateCommand = updateCmd;
                        databaseHelper.adapterRooms.Update(dsModified, "Pokoj");
                    }
                    MessageBox.Show("Zmodyfikowane pokoje zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmian pokoi: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateRoomRow(DataRow row, out string errorMessage)
        {
            errorMessage = "";

            string numerPokoju = row["NumerPokoju"]?.ToString()?.Trim();
            string typPokoju = row["TypPokoju"]?.ToString()?.Trim().ToLower();
            object cenaObj = row["CenaZaNoc"];

            if (string.IsNullOrWhiteSpace(numerPokoju))
            {
                errorMessage = "Numer pokoju nie może być pusty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(typPokoju) ||
                (typPokoju != "standard" && typPokoju != "deluxe" && typPokoju != "penthouse"))
            {
                errorMessage = "Typ pokoju musi być jednym z: Standard, Deluxe, Penthouse.";
                return false;
            }

            if (cenaObj == DBNull.Value || !decimal.TryParse(cenaObj.ToString(), out decimal cena) || cena < 0)
            {
                errorMessage = "Cena za noc musi być liczbą dodatnią.";
                return false;
            }

            return true;
        }

    }
}
