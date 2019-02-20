using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDesktopApp.ORM
{
    public class Uzivatel
    {
        public int ID { get; set; }
        public String Login { get; set; }
        public String Heslo { get; set; }
        public String Type { get; set; }
    }
}
