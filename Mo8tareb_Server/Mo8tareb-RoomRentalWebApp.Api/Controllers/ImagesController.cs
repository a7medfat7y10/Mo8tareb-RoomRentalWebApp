using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.Images;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ImagesManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IimageManager _imageManger;
        private readonly ApplicationDbContext _context;
        public ImagesController(IimageManager imageManager,ApplicationDbContext context)
        {
             _imageManger= imageManager;
            _context= context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Imgs =await _imageManger.GetAllAsync();
            return Ok(Imgs);
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetById(int id)
        {
            var Img = await _imageManger.GetById(id);
            if (Img == null)
                return NotFound();
            return Ok(Img);
        }


        [HttpGet("GetByRoomId")]
        public async Task<IActionResult> GetByRoomId(int id)
        {
            var Imgs = await _imageManger.GetByRoomId(id);
            if (Imgs == null)
                return NotFound();
            return Ok(Imgs);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ImageDto dto)
        {
            if (dto.ImageUrl == null)
                return BadRequest("Image Url Is Required !");

            var isValidRoomId = await _context.Rooms.AnyAsync(r => r.Id == dto.RoomId);
            if (!isValidRoomId)
                return BadRequest("invalid Room Id");

            using var dataStream = new MemoryStream();
            await dto.ImageUrl.CopyToAsync(dataStream);

            var img = new Image
            {
                RoomId = dto.RoomId,
                ImageUrl = dataStream.ToArray()
            };
            await _imageManger.Add(img);
            return Ok(img);

        }


        [HttpPut("Id")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] ImageDto dto)
        {







            var img = await _imageManger.GetById(id);
            if (img == null) return NotFound();

            var isValidRoomId = await _context.Rooms.AnyAsync(r => r.Id == dto.RoomId);
            if (!isValidRoomId)
                return BadRequest("invalid Room Id");

            using var dataStream = new MemoryStream();
            await dto.ImageUrl.CopyToAsync(dataStream);

            img.RoomId = dto.RoomId;
            img.ImageUrl = dataStream.ToArray();

            _imageManger.Update(img);
            return Ok(img);

        }


        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var img = await _imageManger.GetById(id);
            if (img == null) return NotFound();

            await _imageManger.Delete(img);
            return Ok(img);
        }
    }
}
