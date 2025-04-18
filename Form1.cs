﻿using HotelRoomsManagementSystem.Tabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelRoomsManagementSystem
{
    public partial class Form1 : Form
    {
        private DatabaseHelper databaseHelper;
        private BindingSource bindingSourceClients, bindingSourceReservations, bindingSourceRoomServices, bindingSourceRooms;

        private Services services;
        private Reservations reservations;
        private Customers customers;
        private Rooms rooms;

        public Form1()
        {
            databaseHelper = new DatabaseHelper();
            services = new Services();
            reservations = new Reservations();
            customers = new Customers(databaseHelper);
            rooms = new Rooms(databaseHelper);
            InitializeComponent();
            dataGridView_clients.AutoGenerateColumns = true;
            dataGridView_room_services.AutoGenerateColumns = true;
            dataGridView_reservations.AutoGenerateColumns = true;
            dataGridView_rooms.AutoGenerateColumns = true;
            InitializeBindings();
        }

        private void InitializeBindings()
        {
            bindingSourceClients = new BindingSource { DataSource = databaseHelper.dataSet, DataMember = "Klient" };
            bindingSourceReservations = new BindingSource { DataSource = databaseHelper.dataSet, DataMember = "Rezerwacja" };
            bindingSourceRoomServices = new BindingSource { DataSource = databaseHelper.dataSet, DataMember = "Usluga" };
            bindingSourceRooms = new BindingSource { DataSource = databaseHelper.dataSet, DataMember = "Pokoj" };

            dataGridView_clients.DataSource = bindingSourceClients;
            dataGridView_reservations.DataSource = bindingSourceReservations;
            dataGridView_room_services.DataSource = bindingSourceRoomServices;
            dataGridView_rooms.DataSource = bindingSourceRooms;

            bindingNavigator_clients.BindingSource = bindingSourceClients;
            bindingNavigator_reservations.BindingSource = bindingSourceReservations;
            bindingNavigator_room_services.BindingSource = bindingSourceRoomServices;
            bindingNavigator_rooms.BindingSource = bindingSourceRooms;
        }

        private void toolStripBtn_save_clients_Click(object sender, EventArgs e)
        {
            bindingSourceClients.EndEdit();
            this.BindingContext[databaseHelper.dataSet, "Klient"].EndCurrentEdit();

            DataSet ds = databaseHelper.dataSet;

            DataSet dsAdded = ds.GetChanges(DataRowState.Added);
            DataSet dsModified = ds.GetChanges(DataRowState.Modified);
            DataSet dsDeleted = ds.GetChanges(DataRowState.Deleted);

            if (dsAdded != null)
            {
                customers.SaveInsertedClients();
            }
            else if (dsModified != null)
            {
                customers.SaveUpdatedClients();
            }
            else if (dsDeleted != null)
            {
                customers.SaveDeletedClients();
            }
            else
            {
                MessageBox.Show("Brak zmian do zapisania.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            databaseHelper.dataSet.AcceptChanges();
            databaseHelper.ReloadData();
        }

        private void toolStripBtn_save_rooms_Click(object sender, EventArgs e)
        {
            bindingSourceRooms.EndEdit();
            this.BindingContext[databaseHelper.dataSet, "Pokoj"].EndCurrentEdit();

            DataSet ds = databaseHelper.dataSet;

            DataSet dsAdded = ds.GetChanges(DataRowState.Added);
            DataSet dsModified = ds.GetChanges(DataRowState.Modified);

            if (dsAdded != null)
            {
                rooms.SaveInsertedRooms();
            }
            else if (dsModified != null)
            {
                rooms.SaveUpdatedRooms();
            }
            else
            {
                MessageBox.Show("Brak zmian do zapisania.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            databaseHelper.dataSet.AcceptChanges();
            databaseHelper.ReloadData();

        }
    }
}
