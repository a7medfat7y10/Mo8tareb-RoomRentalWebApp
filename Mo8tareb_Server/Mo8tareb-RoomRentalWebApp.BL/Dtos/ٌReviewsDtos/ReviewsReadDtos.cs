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
    public record ReviewsReadDtos(int id, [MaxLength(100)][MinLength(1)] string comment, [Range(0, 10)] int Rating,userReadDtos User,RoomReadDtos? Room);
    public record ReviewsReadDtosV22(int id, [MaxLength(100)][MinLength(1)] string comment, [Range(0, 10)] int Rating,userReadDtos User);


    public class ReviewsReadDtosV2
    {
        public int id { get; set; }

        [MaxLength(100)]
        [MinLength(1)]
        public string? comment { get; set; }

        [Range(0, 10)]
        public int Rating { get; set; }

        public userReadDtosV2? User { get; set; }
        public RoomReadDtosV2? Room { get; set; }
    };

}
