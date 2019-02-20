using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormuleSystem.ORM
{
    public class GP
    {
        public int ID { get; set; }
        public String Nazev { get; set; }
        public DateTime? Datum { get; set; }
        public Double Delka_okruhu { get; set; }
        public int? Pocet_kol { get; set; }
        public int Staty_ID { get; set; }
        public int Uzivatel_ID { get; set; }
    }
}
