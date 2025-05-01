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
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var insertCmd = new OleDbCommand("INSERT INTO Usluga (Nazwa, Opis, Cena) VALUES (?, ?, ?)", conn);
                        insertCmd.Parameters.Add("Nazwa", OleDbType.VarChar, 10, "Nazwa");
                        insertCmd.Parameters.Add("Opis", OleDbType.VarChar, 50, "Opis");
                        insertCmd.Parameters.Add("Cena", OleDbType.Decimal, 0, "Cena");

                        databaseHelper.adapterRooms.InsertCommand = insertCmd;
                        databaseHelper.adapterRooms.Update(dsAdded, "Usluga");
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
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var updateCmd = new OleDbCommand("UPDATE Uslugi SET Nazwa = ?, Opis = ?, Cena = ? WHERE UslugaID = ?", conn);
                        updateCmd.Parameters.Add("UslugaID", OleDbType.Integer, 10, "UslugaID");
                        updateCmd.Parameters.Add("Nazwa", OleDbType.VarChar, 50, "Nazwa");
                        updateCmd.Parameters.Add("Opis", OleDbType.VarChar, 0, "Opis");
                        updateCmd.Parameters.Add("Cena", OleDbType.Decimal, 0, "Cena");

                        databaseHelper.adapterRooms.UpdateCommand = updateCmd;
                        databaseHelper.adapterRooms.Update(dsModified, "Usluga");
                    }
                    MessageBox.Show("Zmodyfikowane uslugi zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmian uslugi: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
        
}
