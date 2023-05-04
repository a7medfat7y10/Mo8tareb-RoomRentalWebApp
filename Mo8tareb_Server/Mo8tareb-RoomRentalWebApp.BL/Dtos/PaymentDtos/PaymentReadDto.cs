using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos
{
    public class PaymentReadDto
    {
        public int Id { get; set; }
        public string? StripeId { get; set; }
        public string? AppUserId { get; set; }
        public int? ReservationId { get; set; }
        public long? Amount { get; set; }
    }

}
