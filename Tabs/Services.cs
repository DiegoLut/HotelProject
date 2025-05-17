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
    public class Services
    {
        private DatabaseHelper databaseHelper;

        public Services(DatabaseHelper dbHelper)
        {
            databaseHelper = dbHelper;
        }

        public void SaveInsertedServices()
        {
            try
            {
                DataSet dsAdded = databaseHelper.dataSet.GetChanges(DataRowState.Added);
                if (dsAdded != null)
                {
                    DataTable uslugiTable = dsAdded.Tables["Usluga"];
                    foreach (DataRow row in uslugiTable.Rows)
                    {
                        if (!ValidateServiceRow(row, isNew: true, out string error))
                        {
                            MessageBox.Show(error, "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var insertCmd = new OleDbCommand("INSERT INTO Usluga (Nazwa, Opis, Cena) VALUES (?, ?, ?)", conn);
                        insertCmd.Parameters.Add("Nazwa", OleDbType.VarChar, 10, "Nazwa");
                        insertCmd.Parameters.Add("Opis", OleDbType.VarChar, 50, "Opis");
                        insertCmd.Parameters.Add("Cena", OleDbType.Decimal, 0, "Cena");

                        databaseHelper.adapterRoomServices.InsertCommand = insertCmd;
                        databaseHelper.adapterRoomServices.Update(dsAdded, "Usluga");
                    }
                    MessageBox.Show("Nowe Uslugi zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu Uslugi: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveUpdatedServices()
        {
            try
            {
                DataSet dsModified = databaseHelper.dataSet.GetChanges(DataRowState.Modified);
                if (dsModified != null)
                {
                    DataTable uslugiTable = dsModified.Tables["Usluga"];
                    foreach (DataRow row in uslugiTable.Rows)
                    {
                        if (!ValidateServiceRow(row, isNew: false, out string error))
                        {
                            MessageBox.Show(error, "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var updateCmd = new OleDbCommand("UPDATE Uslugi SET Nazwa = ?, Opis = ?, Cena = ? WHERE UslugaID = ?", conn);
                        updateCmd.Parameters.Add("UslugaID", OleDbType.Integer, 10, "UslugaID");
                        updateCmd.Parameters.Add("Nazwa", OleDbType.VarChar, 50, "Nazwa");
                        updateCmd.Parameters.Add("Opis", OleDbType.VarChar, 0, "Opis");
                        updateCmd.Parameters.Add("Cena", OleDbType.Decimal, 0, "Cena");

                        databaseHelper.adapterRoomServices.UpdateCommand = updateCmd;
                        databaseHelper.adapterRoomServices.Update(dsModified, "Usluga");
                    }
                    MessageBox.Show("Zmodyfikowane uslugi zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmian uslugi: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateServiceRow(DataRow row, bool isNew, out string errorMessage)
        {
            errorMessage = "";
            var ds = databaseHelper.dataSet;

            string nazwa = row["Nazwa"]?.ToString()?.Trim();
            string opis = row["Opis"]?.ToString()?.Trim();
            object cenaObj = row["Cena"];

            if (string.IsNullOrWhiteSpace(nazwa))
            {
                errorMessage = "Nazwa usługi nie może być pusta.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(opis))
            {
                errorMessage = "Opis usługi nie może być pusty.";
                return false;
            }

            if (cenaObj == DBNull.Value || !decimal.TryParse(cenaObj.ToString(), out decimal cena) || cena < 0)
            {
                errorMessage = "Cena musi być liczbą dodatnią.";
                return false;
            }

 
            if (isNew)
            {
                bool exists = ds.Tables["Usluga"]
                    .AsEnumerable()
                    .Any(r => r.RowState != DataRowState.Deleted && r["Nazwa"].ToString().Equals(nazwa, StringComparison.OrdinalIgnoreCase));

                if (exists)
                {
                    errorMessage = $"Usługa o nazwie '{nazwa}' już istnieje.";
                    return false;
                }
            }

            return true;
        }

    }

}
