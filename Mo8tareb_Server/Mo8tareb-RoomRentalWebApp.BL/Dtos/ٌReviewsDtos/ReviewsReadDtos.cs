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
    public record ReviewsReadDtos(int id, [MaxLength(100)][MinLength(1)] string comment, [Range(0, 10)] int Rating,userReadDtos User,RoomReadDtos Room);



}
