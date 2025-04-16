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
        DatabaseHelper databaseHelper = new DatabaseHelper();
        public void SaveClientsChanges()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString))
                {
                    conn.Open();

                    string insert = "INSERT INTO Klient (Imie, Nazwisko, Email, Telefon) VALUES (?, ?, ?, ?)";
                    var cmd = new OleDbCommand(insert, conn);

                    cmd.Parameters.Add("Imie", OleDbType.VarChar, 50, "Imie");
                    cmd.Parameters.Add("Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                    cmd.Parameters.Add("Email", OleDbType.VarChar, 100, "Email");
                    cmd.Parameters.Add("Telefon", OleDbType.VarChar, 20, "Telefon");

                    databaseHelper.adapterClients.InsertCommand = cmd;

                    DataSet dsBefore = new DataSet();
                    databaseHelper.adapterClients.Fill(dsBefore, "Klient");

                    DataRow newRow = dsBefore.Tables["Klient"].NewRow();
                    newRow["Imie"] = "Anna";
                    newRow["Nazwisko"] = "Nowak";
                    newRow["Email"] = "na.nowak@example.com";
                    newRow["Telefon"] = "123456789";

                    dsBefore.Tables["Klient"].Rows.Add(newRow);
                    if (dsBefore.HasChanges())
                    {
                        DataSet dsChanges = dsBefore.GetChanges();
                        
                        databaseHelper.adapterClients.Update(dsChanges, "Klient");
                        dsBefore.AcceptChanges();

                        MessageBox.Show("Klient został zapisany do bazy!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        

                    }
                    else
                    {
                        MessageBox.Show("Brak zmian do zapisania", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /* try
             {
                 OleDbConnection conn = new OleDbConnection(databaseHelper.connectionString);

                 var dataSetBefore = new DataSet();
                 var dataSetAfter = new DataSet();

                 string insert = "INSERT INTO Klient (Imie, Nazwisko, Email, Telefon) VALUES (@Imie, @Nazwisko, @Email, @Telefon)";
                 var cmd = new OleDbCommand(insert, conn);


                 cmd.Parameters.Add("@Imie", OleDbType.VarChar, 50, "Imie");
                 cmd.Parameters.Add("@Nazwisko", OleDbType.VarChar, 50, "Nazwisko");
                 cmd.Parameters.Add("@Email", OleDbType.VarChar, 100, "Email");
                 cmd.Parameters.Add("@Telefon", OleDbType.VarChar, 20, "Telefon");

                 databaseHelper.adapterClients.InsertCommand = cmd;

                 databaseHelper.adapterClients.Fill(dataSetBefore, "Klient");


                 if (dataSetBefore.HasChanges())
                 {
                     dataSetAfter = dataSetBefore.GetChanges();
                     databaseHelper.adapterClients.Update(dataSetAfter, "Klient");

                     MessageBox.Show("Zmiany zostaly zapisane");
                 }
                 else
                 {
                     MessageBox.Show("Wprowadz zmiany przed zapisem");
                 }

             } 
             catch(Exception ex)
             {
                 // Handle exception
                 Console.WriteLine("Blad: " + ex.Message);
             }   */
        }
    }

    public class CustomerValidations
    {

    }


}