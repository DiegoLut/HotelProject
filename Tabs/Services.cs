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
                        var insertCmd = new OleDbCommand("INSERT INTO Uslugi (Nazwa, Opis, Cena) VALUES (?, ?, ?)", conn);
                        insertCmd.Parameters.Add("Nazwa", OleDbType.VarChar, 10, "Nazwa");
                        insertCmd.Parameters.Add("Opis", OleDbType.VarChar, 50, "Opis");
                        insertCmd.Parameters.Add("Cena", OleDbType.Currency, 0, "Cena");

                        databaseHelper.adapterRooms.InsertCommand = insertCmd;
                        databaseHelper.adapterRooms.Update(dsAdded, "Uslugi");
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
                        var updateCmd = new OleDbCommand("UPDATE Pokoj SET NumerPokoju = ?, TypPokoju = ?, CenaZaNoc = ?, Dostepnosc = ? WHERE PokojID = ?", conn);
                        updateCmd.Parameters.Add("NumerPokoju", OleDbType.VarChar, 10, "NumerPokoju");
                        updateCmd.Parameters.Add("TypPokoju", OleDbType.VarChar, 50, "TypPokoju");
                        updateCmd.Parameters.Add("CenaZaNoc", OleDbType.Currency, 0, "CenaZaNoc");
                        updateCmd.Parameters.Add("Dostepnosc", OleDbType.Boolean, 0, "Dostepnosc");
                        updateCmd.Parameters.Add("PokojID", OleDbType.Integer, 0, "PokojID").SourceVersion = DataRowVersion.Original;

                        databaseHelper.adapterRooms.UpdateCommand = updateCmd;
                        databaseHelper.adapterRooms.Update(dsModified, "Pokoj");
                    }
                    MessageBox.Show("Zmodyfikowane Uslugi zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmian pokoi: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
        
}
