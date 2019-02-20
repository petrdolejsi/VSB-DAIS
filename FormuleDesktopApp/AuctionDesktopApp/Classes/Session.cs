using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDesktopApp.Classes
{
    public static class Session
    {
        /// <summary>
        /// ID of the currently logged user.
        /// </summary>
        public static int LoggedUserId
        {
            get
            {
                // TODO - should be determined from a logged user.
                return 1;
            }
        }
    }
}
