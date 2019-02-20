using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormuleSystem.ORM
{
    public class Jezdci
    {
        public int ID { get; set; }
        public String Jmeno { get; set; }
        public String Prijmeni { get; set; }
        public int? Startovni_cislo { get; set; }
        public DateTime? Datum_narozeni { get; set; }
        public int? Tymy_ID { get; set; }
        public int? Staty_ID { get; set; }
        public int? Motory_Seriove_cislo { get; set; }
        public int? Suma { get; set; }
    }
}
