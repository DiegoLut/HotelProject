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
                        var insertCmd = new OleDbCommand("INSERT INTO Rezerwacja (PokojID, KlientID, DataZameldowania, DataWymeldowania, Cena, Rabat) VALUES (?, ?, ?, ?, ?, ?)",conn);
                        insertCmd.Parameters.Add("PokojID", OleDbType.Integer, 10, "PokojID");
                        insertCmd.Parameters.Add("KlientID", OleDbType.Integer, 10, "KlientID");
                        insertCmd.Parameters.Add("DataZameldowania", OleDbType.VarChar, 10, "DataZameldowania");
                        insertCmd.Parameters.Add("DataWymeldowania", OleDbType.VarChar, 50, "DataWymeldowania");
                        insertCmd.Parameters.Add("Cena", OleDbType.Decimal, 0, "Cena");
                        insertCmd.Parameters.Add("Rabat", OleDbType.Decimal, 0, "Rabat");

                        databaseHelper.adapterRooms.InsertCommand = insertCmd;
                        databaseHelper.adapterRooms.Update(dsAdded, "Rezerwacja");
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
                        var updateCmd = new OleDbCommand("UPDATE Rezerwacja SET DataZameldowania = ?, DataWymeldowania = ?, Cena = ?, Rabat = ? WHERE RezerwacjaID = ?", conn);
                        updateCmd.Parameters.Add("DataZameldowania", OleDbType.VarChar, 10, "DataZameldowania");
                        updateCmd.Parameters.Add("DataWymeldowania", OleDbType.VarChar, 50, "DataWymeldowania");
                        updateCmd.Parameters.Add("Cena", OleDbType.Currency, 0, "Cena");
                        updateCmd.Parameters.Add("Rabat", OleDbType.Boolean, 0, "Rabat");

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
    }




}
