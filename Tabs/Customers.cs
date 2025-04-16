using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HotelRoomsManagementSystem.Tabs
{
    public class Customers
    {
        private DatabaseHelper databaseHelper;

        public Customers(DatabaseHelper dbHelper)
        {
            databaseHelper = dbHelper;
        }

        public void SaveInsertedClients()
        {
            try
            {
                DataSet dsAdded = databaseHelper.dataSet.GetChanges(DataRowState.Added);
                if (dsAdded != null)
                {
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        conn.Open();

                        var insertCmd = new OleDbCommand("INSERT INTO Klient (Imie, Nazwisko, Email, Telefon) VALUES (?, ?, ?, ?)", conn);
                        insertCmd.Parameters.Add("Imie", OleDbType.VarChar, 50, "Imie");
                        insertCmd.Parameters.Add("Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                        insertCmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                        insertCmd.Parameters.Add("Telefon", OleDbType.VarChar, 20, "Telefon");
                        databaseHelper.adapterClients.InsertCommand = insertCmd;

                        databaseHelper.adapterClients.Update(dsAdded, "Klient");
                        conn.Close();
                    }
                    MessageBox.Show("Nowe rekordy zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu nowych rekordów: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveUpdatedClients()
        {
            try
            {
                DataSet dsModified = databaseHelper.dataSet.GetChanges(DataRowState.Modified);
                if (dsModified != null)
                {
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        conn.Open();
                        var updateCmd = new OleDbCommand("UPDATE Klient SET Imie = ?, Nazwisko = ?, Email = ?, Telefon = ? WHERE KlientID = ?", conn);
                        updateCmd.Parameters.Add("Imie", OleDbType.VarChar, 50, "Imie");
                        updateCmd.Parameters.Add("Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                        updateCmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                        updateCmd.Parameters.Add("Telefon", OleDbType.VarChar, 20, "Telefon");
                        updateCmd.Parameters.Add("KlientID", OleDbType.Integer, 0, "KlientID").SourceVersion = DataRowVersion.Original;
                        databaseHelper.adapterClients.UpdateCommand = updateCmd;

                        databaseHelper.adapterClients.Update(dsModified, "Klient");
                        conn.Close();
                    }
                    MessageBox.Show("Zmodyfikowane rekordy zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu zmodyfikowanych rekordów: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveDeletedClients()
        {
            try
            {
                DataSet dsDeleted = databaseHelper.dataSet.GetChanges(DataRowState.Deleted);
                if (dsDeleted != null)
                {
                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {
                        conn.Open();
                        var deleteCmd = new OleDbCommand("DELETE FROM Klient WHERE KlientID = ?", conn);
                        deleteCmd.Parameters.Add("KlientID", OleDbType.Integer, 0, "KlientID").SourceVersion = DataRowVersion.Original;
                        databaseHelper.adapterClients.DeleteCommand = deleteCmd;

                        databaseHelper.adapterClients.Update(dsDeleted, "Klient");
                        conn.Close();
                    }
                    MessageBox.Show("Usunięte rekordy zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu usuniętych rekordów: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /*public void SaveClientsChanges()
    {
        try
        {
            using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
            {
                conn.Open();

                // Komendy INSERT
                var insertCmd = new OleDbCommand("INSERT INTO Klient (Imie, Nazwisko, Email, Telefon) VALUES (?, ?, ?, ?)", conn);
                insertCmd.Parameters.Add("Imie", OleDbType.VarChar, 50, "Imie");
                insertCmd.Parameters.Add("Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                insertCmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                insertCmd.Parameters.Add("Telefon", OleDbType.VarChar, 20, "Telefon");
                databaseHelper.adapterClients.InsertCommand = insertCmd;

                // Komenda UPDATE
                var updateCmd = new OleDbCommand("UPDATE Klient SET Imie = ?, Nazwisko = ?, Email = ?, Telefon = ? WHERE KlientID = ?", conn);
                updateCmd.Parameters.Add("Imie", OleDbType.VarChar, 50, "Imie");
                updateCmd.Parameters.Add("Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                updateCmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                updateCmd.Parameters.Add("Telefon", OleDbType.VarChar, 20, "Telefon");
                updateCmd.Parameters.Add("KlientID", OleDbType.Integer, 0, "KlientID").SourceVersion = DataRowVersion.Original;
                databaseHelper.adapterClients.UpdateCommand = updateCmd;

                // Komenda DELETE
                var deleteCmd = new OleDbCommand("DELETE FROM Klient WHERE KlientID = ?", conn);
                deleteCmd.Parameters.Add("KlientID", OleDbType.Integer, 0, "KlientID").SourceVersion = DataRowVersion.Original;
                databaseHelper.adapterClients.DeleteCommand = deleteCmd;

                DataSet dataSetBefore = databaseHelper.dataSet;
                if (dataSetBefore.HasChanges())
                {
                    Console.WriteLine("Są zmiany do zapisania.");

                    DataSet dataSetAfter = dataSetBefore.GetChanges();

                    databaseHelper.adapterClients.Update(dataSetAfter, "Klient");
                    dataSetBefore.AcceptChanges();
                    MessageBox.Show("Zmiany zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Brak zmian do zapisania.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd zapisu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}*/

    public class CustomerValidations
    {

    }
}


