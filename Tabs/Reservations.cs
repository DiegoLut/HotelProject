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
        public void SaveReservationsChanges()
        {
            try
            {
                DataSet dsAdded = databaseHelper.dataSet.GetChanges(DataRowState.Added);
                if (dsAdded != null)
                {
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        var insertCmd = new OleDbCommand("INSERT INTO Reservations (DataZameldowania, DataWymeldowania, Cena, Rabat) VALUES (?, ?, ?, ?)", conn);
                        insertCmd.Parameters.Add("DataZameldowania", OleDbType.VarChar, 10, "NumerPokoju");
                        insertCmd.Parameters.Add("DataWymeldowania", OleDbType.VarChar, 50, "TypPokoju");
                        insertCmd.Parameters.Add("Cena", OleDbType.Currency, 0, "CenaZaNoc");
                        insertCmd.Parameters.Add("Rabat", OleDbType.Boolean, 0, "Dostepnosc");

                        databaseHelper.adapterRooms.InsertCommand = insertCmd;
                        databaseHelper.adapterRooms.Update(dsAdded, "Rezerwacje");
                    }
                    MessageBox.Show("Nowe Rezerwacje zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu pokoi: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        var updateCmd = new OleDbCommand("UPDATE Rezerwacje SET DataZameldowania = ?, DataWymeldowania = ?, Cena = ?, Rabat = ? WHERE PokojID = ?", conn);
                        updateCmd.Parameters.Add("DataZameldowania", OleDbType.VarChar, 10, "NumerPokoju");
                        updateCmd.Parameters.Add("DataWymeldowania", OleDbType.VarChar, 50, "TypPokoju");
                        updateCmd.Parameters.Add("Cena", OleDbType.Currency, 0, "CenaZaNoc");
                        updateCmd.Parameters.Add("Rabat", OleDbType.Boolean, 0, "Dostepnosc");

                        databaseHelper.adapterReservations.UpdateCommand = updateCmd;
                        databaseHelper.adapterRooms.Update(dsModified, "Rezerwacje");
                    }
                    MessageBox.Show("Zmodyfikowane Rezerwacje zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmian Rezerwacji: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }




}
