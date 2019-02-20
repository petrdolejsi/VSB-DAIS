using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormuleSystem.ORM
{
    public class Tymy
    {
        public int ID { get; set; }
        public String Nazev { get; set; }
        public String Dodavatel_pneumatik { get; set; }
        public int? Vyrobce_motoru_ID { get; set; }
        public int? Staty_ID { get; set; }
        public int? Uzivatel_ID { get; set; }
        public int? Suma { get; set; }
    }
}
