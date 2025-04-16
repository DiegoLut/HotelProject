using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomsManagementSystem.Tabs
{
    public class Services
    {
        DatabaseHelper databaseHelper = new DatabaseHelper();

        public void SaveRoomServicesChanges()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(databaseHelper.connectionString))
                {
              //      databaseHelper.adapterRoomServices.InsertCommand = new SqlCommand(
              //          @"INSERT INTO Usluga (Nazwa, Opis, Cena) 
              //VALUES (@Nazwa, @Opis, @Cena)", conn);
              //      databaseHelper.adapterRoomServices.InsertCommand.Parameters.Add("@Nazwa", SqlDbType.NVarChar, 100, "Nazwa");
              //      databaseHelper.adapterRoomServices.InsertCommand.Parameters.Add("@Opis", SqlDbType.NVarChar, 255, "Opis");
              //      databaseHelper.adapterRoomServices.InsertCommand.Parameters.Add("@Cena", SqlDbType.Decimal, 0, "Cena");


                    //databaseHelper.adapterRoomServices.Update(databaseHelper.dataSet, "Usluga");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Blad: " + ex.Message);
            }
        }
    }
        
}
