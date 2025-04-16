using HotelRoomsManagementSystem.Tabs;
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

        public Form1()
        {
            databaseHelper = new DatabaseHelper();
            services = new Services();
            reservations = new Reservations();
            customers = new Customers(databaseHelper);
            InitializeComponent();
            dataGridView_clients.AutoGenerateColumns = true;
            dataGridView_room_services.AutoGenerateColumns = true;
            dataGridView_reservations.AutoGenerateColumns = true;
            dataGridView_rooms.AutoGenerateColumns = true;
            dataGridView_reservations.EditingControlShowing += DataGridView_reservations_EditingControlShowing;
            dataGridView_reservations.DataError += DataGridView_reservations_DataError;
            ShowDateTimePickerForColumns();
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

        private void ShowDateTimePickerForColumns()
        {
            foreach (DataGridViewRow row in dataGridView_reservations.Rows)
            {
                if (row.Cells["DataZameldowania"] != null && row.Cells["DataZameldowania"].Value != DBNull.Value)
                {
                    DateTime? dataZameldowania = row.Cells["DataZameldowania"].Value as DateTime?;
                    if (dataZameldowania == null || dataZameldowania == DateTime.MinValue)
                    {
                        dataZameldowania = DateTime.Now;
                    }

                    DateTimePicker dtp = new DateTimePicker();
                    dtp.Format = DateTimePickerFormat.Short;
                    Rectangle rect = dataGridView_reservations.GetCellDisplayRectangle(
                        row.Cells["DataZameldowania"].ColumnIndex, row.Index, true);
                    dtp.Size = rect.Size;
                    dtp.Location = rect.Location;
                    dtp.Value = dataZameldowania.Value;
                    dataGridView_reservations.Controls.Add(dtp);
                }

                if (row.Cells["DataWymeldowania"] != null && row.Cells["DataWymeldowania"].Value != DBNull.Value)
                {
                    DateTime? dataWymeldowania = row.Cells["DataWymeldowania"].Value as DateTime?;
                    if (dataWymeldowania == null || dataWymeldowania == DateTime.MinValue)
                    {
                        dataWymeldowania = DateTime.Now;
                    }

                    DateTimePicker dtp = new DateTimePicker();
                    dtp.Format = DateTimePickerFormat.Short;
                    Rectangle rect = dataGridView_reservations.GetCellDisplayRectangle(
                        row.Cells["DataWymeldowania"].ColumnIndex, row.Index, true);
                    dtp.Size = rect.Size;
                    dtp.Location = rect.Location;
                    dtp.Value = dataWymeldowania.Value;
                    dataGridView_reservations.Controls.Add(dtp);
                }
            }
        }

        private void DataGridView_reservations_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView_reservations.CurrentCell.OwningColumn.Name == "DataZameldowania" ||
                dataGridView_reservations.CurrentCell.OwningColumn.Name == "DataWymeldowania")
            {
                DateTimePicker dtp = e.Control as DateTimePicker;
                if (dtp != null)
                {
                    dtp.Format = DateTimePickerFormat.Short;
                    dtp.CloseUp += (s, args) =>
                    {
                        if (!DateTime.TryParse(dtp.Text, out _))
                        {
                            MessageBox.Show("Niepoprawny format daty. Proszę wprowadzić datę w formacie dd/mm/yyyy.", "Błąd formatu daty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dtp.Focus();
                        }
                    };
                }
            }
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




        /*
         *  try
            {
                int selectedIndex = tabControl1.SelectedIndex;

                switch (selectedIndex)
                {
                    case 0:
                        customers.SaveClientsChanges();
                        break;

                    case 1:
                        reservations.SaveReservationsChanges();
                        break;

                    case 2:
                        services.SaveRoomServicesChanges();
                        break;

                    default:
                        MessageBox.Show("Nieobsługiwana zakładka.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }

                //MessageBox.Show("Zmiany zapisane pomyślnie! ", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         */



        private void DataGridView_reservations_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is FormatException)
            {
                MessageBox.Show("Niepoprawny format daty w polu: " + dataGridView_reservations.Columns[e.ColumnIndex].HeaderText,
                                "Błąd formatu",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                e.ThrowException = false;
            }
        }


        private void button_add_Click(object sender, EventArgs e)
        {
            
        }

    }
}
