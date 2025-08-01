using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace SportBooking_WPF.Helpers
{
    public class LoggedInUser
    {
        private static LoggedInUser _instance;
        public static LoggedInUser Instance => _instance ??= new LoggedInUser();

        public User? User { get; set; }

        private LoggedInUser() { }
    }
}
