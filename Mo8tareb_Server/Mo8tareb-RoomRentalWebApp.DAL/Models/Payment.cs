using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string? StripeId { get; set; }
      //  public string Status { get; set; } = string.Empty;

      
        public int? ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public virtual Reservation? Reservation { get; set; }

        [DataType(DataType.Currency)]
        public long? Amount { get; set; }
    }
}
