using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomsManagementSystem.Model
{
    internal class RezerwacjaUsluga
    {
        public int RezerwacjaUslugaID { get; set; }
        public int RezerwacjaID { get; set; }
        public int UslugaID { get; set; }
        public int Ilosc { get; set; } = 1;
        public decimal CenaLaczna { get; set; }
    }
}
