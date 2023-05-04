using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos
{
    public record OwnerReadDtos(string Email, string firstName, string lastName);
    public class OwnerReadDtosV2
    {
        public string? Email { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? PhoneNumber { get; set; } = String.Empty;
    }
    public record OwnerCreateDtos(string Email);


}
