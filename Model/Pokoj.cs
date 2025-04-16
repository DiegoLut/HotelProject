using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomsManagementSystem.Model
{
    internal class Pokoj
    {
        public int PokojID { get; set; }
        public string NumerPokoju { get; set; }
        public string TypPokoju { get; set; }
        public decimal CenaZaNoc { get; set; }
        public bool Dostepnosc { get; set; } = true;
    }
}
