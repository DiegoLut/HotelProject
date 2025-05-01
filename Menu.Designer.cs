namespace HotelRoomsManagementSystem
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_guests = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_reservations = new System.Windows.Forms.Button();
            this.btn_room_services = new System.Windows.Forms.Button();
            this.btn_rooms = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_guests
            // 
            this.btn_guests.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btn_guests.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_guests.Location = new System.Drawing.Point(360, 235);
            this.btn_guests.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_guests.Name = "btn_guests";
            this.btn_guests.Size = new System.Drawing.Size(180, 76);
            this.btn_guests.TabIndex = 0;
            this.btn_guests.Text = "Goście";
            this.btn_guests.UseVisualStyleBackColor = false;
            this.btn_guests.Click += new System.EventHandler(this.btn_guests_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(473, 110);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 54);
            this.label1.TabIndex = 4;
            this.label1.Text = "Witaj!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_reservations
            // 
            this.btn_reservations.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btn_reservations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_reservations.Location = new System.Drawing.Point(591, 235);
            this.btn_reservations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_reservations.Name = "btn_reservations";
            this.btn_reservations.Size = new System.Drawing.Size(180, 76);
            this.btn_reservations.TabIndex = 5;
            this.btn_reservations.Text = "Rezerwacje";
            this.btn_reservations.UseVisualStyleBackColor = false;
            this.btn_reservations.Click += new System.EventHandler(this.btn_reservations_Click);
            // 
            // btn_room_services
            // 
            this.btn_room_services.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btn_room_services.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_room_services.Location = new System.Drawing.Point(360, 350);
            this.btn_room_services.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_room_services.Name = "btn_room_services";
            this.btn_room_services.Size = new System.Drawing.Size(180, 76);
            this.btn_room_services.TabIndex = 6;
            this.btn_room_services.Text = "Usługi";
            this.btn_room_services.UseVisualStyleBackColor = false;
            this.btn_room_services.Click += new System.EventHandler(this.btn_room_services_Click);
            // 
            // btn_rooms
            // 
            this.btn_rooms.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btn_rooms.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_rooms.Location = new System.Drawing.Point(591, 350);
            this.btn_rooms.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_rooms.Name = "btn_rooms";
            this.btn_rooms.Size = new System.Drawing.Size(180, 76);
            this.btn_rooms.TabIndex = 7;
            this.btn_rooms.Text = "Pokoje";
            this.btn_rooms.UseVisualStyleBackColor = false;
            this.btn_rooms.Click += new System.EventHandler(this.btn_rooms_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Indigo;
            this.ClientSize = new System.Drawing.Size(1157, 598);
            this.Controls.Add(this.btn_rooms);
            this.Controls.Add(this.btn_room_services);
            this.Controls.Add(this.btn_reservations);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_guests);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_guests;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_reservations;
        private System.Windows.Forms.Button btn_room_services;
        private System.Windows.Forms.Button btn_rooms;
    }
}