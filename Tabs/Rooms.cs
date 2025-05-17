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
                        if (!ValidateServiceRow(row, true, out string errorMessage))
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
                        if (!ValidateServiceRow(row, false, out string errorMessage))
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

        private bool ValidateServiceRow(DataRow row, bool isNew, out string errorMessage)
        {
            errorMessage = "";

            string nazwa = row["Nazwa"]?.ToString()?.Trim();
            string typPokoju = row["TypPokoju"]?.ToString()?.Trim().ToLower();
            object cenaObj = row["Cena"];

            if (string.IsNullOrWhiteSpace(nazwa))
            {
                errorMessage = "Nazwa usługi nie może być pusta.";
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
                errorMessage = "Cena musi być liczbą dodatnią.";
                return false;
            }

            if (isNew)
            {
                foreach (DataRow existingRow in databaseHelper.dataSet.Tables["Usluga"].Rows)
                {
                    if (existingRow.RowState != DataRowState.Deleted &&
                        existingRow["Nazwa"].ToString().Trim().Equals(nazwa, StringComparison.OrdinalIgnoreCase))
                    {
                        errorMessage = $"Usługa o nazwie '{nazwa}' już istnieje.";
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
