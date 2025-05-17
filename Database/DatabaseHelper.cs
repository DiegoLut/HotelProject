using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Windows.Forms;

namespace HotelRoomsManagementSystem
{

    public class DatabaseHelper
    {
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString;

        public DataSet dataSet;

        public OleDbDataAdapter adapterClients, adapterReservations, adapterRoomServices, adapterRooms, adapterReservationsWithServices;

        public OleDbCommandBuilder builderClients, builderReservations, builderRoomServices, builderRooms, builderReservationsWithServices;



        public DatabaseHelper()
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                adapterClients = new OleDbDataAdapter("SELECT * FROM Klient", connectionString);
                adapterReservations = new OleDbDataAdapter("SELECT r.RezerwacjaID, r.NumerPokoju, r.Email, r.DataZameldowania, r.DataWymeldowania, r.Rabat, ru.RezerwacjaUslugaID, ru.NazwaUslugi, " +
                    "ru.CenaLaczna FROM Rezerwacja AS r JOIN RezerwacjaUsluga AS ru ON r.RezerwacjaID = ru.RezerwacjaID", connectionString);
                adapterReservationsWithServices = new OleDbDataAdapter("SELECT * FROM RezerwacjaUsluga", connectionString);
                adapterRoomServices = new OleDbDataAdapter("SELECT * FROM Usluga", connectionString);
                adapterRooms = new OleDbDataAdapter("SELECT * FROM Pokoj", connectionString);


                dataSet = new DataSet();
                adapterClients.Fill(dataSet, "Klient");
                adapterReservations.Fill(dataSet, "Rezerwacja");
                adapterReservationsWithServices.Fill(dataSet, "RezerwacjaUsluga");
                adapterRoomServices.Fill(dataSet, "Usluga");
                adapterRooms.Fill(dataSet, "Pokoj");


                builderClients = new OleDbCommandBuilder(adapterClients);
                builderReservations = new OleDbCommandBuilder(adapterReservations);
                builderRoomServices = new OleDbCommandBuilder(adapterRoomServices);
                builderReservationsWithServices = new OleDbCommandBuilder(adapterReservationsWithServices);
                builderRooms = new OleDbCommandBuilder(adapterRooms);


            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad: " + ex.Message);
            }
        }


        public void ReloadData()
        {
            try
            {
                dataSet.Clear();
                adapterClients.Fill(dataSet, "Klient");
                adapterReservations.Fill(dataSet, "Rezerwacja");
                adapterRoomServices.Fill(dataSet, "Usluga");
                adapterRooms.Fill(dataSet, "Pokoj");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odświeżania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
