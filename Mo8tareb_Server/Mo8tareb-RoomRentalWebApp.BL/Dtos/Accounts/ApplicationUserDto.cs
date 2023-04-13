using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts
{
    public class ApplicationUserDto
    {

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

}
