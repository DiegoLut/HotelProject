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
    public partial class Menu : Form
    {
        private Form1 mainForm;

        public Menu()
        {
            InitializeComponent();
            mainForm = new Form1(this);
        }

        private void btn_guests_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            mainForm.tabControl1.SelectedTab = mainForm.tabControl1.TabPages["tab_clients"];
            this.Hide();
        }

        private void btn_reservations_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            mainForm.tabControl1.SelectedTab = mainForm.tabControl1.TabPages["tab_reservations"];
            this.Hide();
        }

        private void btn_room_services_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            mainForm.tabControl1.SelectedTab = mainForm.tabControl1.TabPages["tab_roomServices"];
            this.Hide();
        }

        private void btn_rooms_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            mainForm.tabControl1.SelectedTab = mainForm.tabControl1.TabPages["tab_rooms"];
            this.Hide();
        }
    }
}
