using Microsoft.AspNetCore.Http;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Images
{
    public class ImageDto
    {
    
        public int RoomId { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
