using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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
                    foreach (DataRow row in dsAdded.Tables["Klient"].Rows)
                    {
                        string errorMessage;
                        if (!ValidateClientRow(row, out errorMessage))
                        {
                            MessageBox.Show("Błąd walidacji: " + errorMessage, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                    {

                        var insertCmd = new OleDbCommand("INSERT INTO Klient (Imie, Nazwisko, Email, Telefon) VALUES (?, ?, ?, ?)", conn);
                        insertCmd.Parameters.Add("Imie", OleDbType.VarChar, 50, "Imie");
                        insertCmd.Parameters.Add("Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                        insertCmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                        insertCmd.Parameters.Add("Telefon", OleDbType.VarChar, 20, "Telefon");
                        databaseHelper.adapterClients.InsertCommand = insertCmd;

                        databaseHelper.adapterClients.Update(dsAdded, "Klient");
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
                        var updateCmd = new OleDbCommand("UPDATE Klient SET Imie = ?, Nazwisko = ?, Email = ?, Telefon = ? WHERE KlientID = ?", conn);
                        updateCmd.Parameters.Add("Imie", OleDbType.VarChar, 50, "Imie");
                        updateCmd.Parameters.Add("Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                        updateCmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                        updateCmd.Parameters.Add("Telefon", OleDbType.VarChar, 20, "Telefon");
                        updateCmd.Parameters.Add("KlientID", OleDbType.Integer, 0, "KlientID").SourceVersion = DataRowVersion.Original;
                        databaseHelper.adapterClients.UpdateCommand = updateCmd;

                        databaseHelper.adapterClients.Update(dsModified, "Klient");
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
                        var deleteCmd = new OleDbCommand("DELETE FROM Klient WHERE KlientID = ?", conn);
                        deleteCmd.Parameters.Add("KlientID", OleDbType.Integer, 0, "KlientID").SourceVersion = DataRowVersion.Original;
                        databaseHelper.adapterClients.DeleteCommand = deleteCmd;

                        databaseHelper.adapterClients.Update(dsDeleted, "Klient");
                    }
                    MessageBox.Show("Usunięte rekordy zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu usuniętych rekordów: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateClientRow(DataRow row, out string errorMessage)
        {
            errorMessage = string.Empty;

            string imie = row["Imie"]?.ToString().Trim();
            string nazwisko = row["Nazwisko"]?.ToString().Trim();
            string email = row["Email"]?.ToString().Trim();
            string telefon = row["Telefon"]?.ToString().Trim();

            if (!CustomerValidations.ValidateName(imie))
            {
                errorMessage = "Imię może zawierać tylko litery i opcjonalne spacje.";
                return false;
            }

            if (!CustomerValidations.ValidateName(nazwisko))
            {
                errorMessage = "Nazwisko może zawierać tylko litery i opcjonalne spacje.";
                return false;
            }

            if (!CustomerValidations.ValidateEmail(email))
            {
                errorMessage = "Adres email jest niepoprawny.";
                return false;
            }

            if (!CustomerValidations.ValidatePhone(telefon))
            {
                errorMessage = "Numer telefonu jest niepoprawny.";
                return false;
            }
            return true;
        }
    }
    public class CustomerValidations
    {
        public static bool ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            string pattern = @"^[\p{L}]+(?: [\p{L}]+)*$";
            return Regex.IsMatch(name, pattern);
        }

        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\+?\d{9,15}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}


