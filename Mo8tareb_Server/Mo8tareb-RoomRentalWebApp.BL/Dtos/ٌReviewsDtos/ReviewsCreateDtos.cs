using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos
{
    public record ReviewsCreateDtos([MaxLength(100)] [MinLength(1)] string comment,[Range(0,10)] int Rating, userCreateDtos User, RoomCreateDtos Room);
    public record ReviewsUpdateDtos(int id, [MaxLength(100)][MinLength(1)] string comment, [Range(0, 10)] int Rating);
    public record ReviewsToDeleteDtos(int id);
    public class CreateReviewPayload
    {
        public int Rating { get; set; }
        public string? UserEmail { get; set; }
        public string? Comments { get; set; }
        public int RoomId { get; set; }
        //public int ReservationId { get; set; }
    }

}
