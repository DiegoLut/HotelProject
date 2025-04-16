using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomsManagementSystem.Model
{
    internal class Rezerwacja
    {
        public int RezerwacjaID { get; set; }
        public int PokojID { get; set; }
        public int KlientID { get; set; }
        public DateTime DataZameldowania { get; set; }
        public DateTime DataWymeldowania { get; set; }
        public decimal Cena { get; set; } = 0;
        public decimal Rabat { get; set; } = 0;
    }
}
