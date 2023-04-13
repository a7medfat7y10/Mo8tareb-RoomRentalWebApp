using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts
{

    public class ExternalAuthDto
    {
        
        public string Provider { get; set; } = String.Empty;
        public string IdToken { get; set; } = String.Empty;
    }

}
