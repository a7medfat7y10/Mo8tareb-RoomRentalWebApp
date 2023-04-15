using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ImagesManagers
{
    public class ImageManager:IimageManager
    {
        private readonly ApplicationDbContext _context;
        public ImageManager(ApplicationDbContext context)
        {
            _context = context;
        }

     
       
        public async Task<IEnumerable<Image>> GetAllAsync()
        {
          return  await _context.Images.ToListAsync();
        }

        public async Task<Image> GetById(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task<IEnumerable<Image>> GetByRoomId(int id)
        {
            return await _context.Images.Where(r => r.RoomId == id).ToListAsync();
        }
        public async Task Add(Image image)
        {
            await _context.AddAsync(image);
            _context.SaveChanges();
        }


        public Image Update(Image image)
        {
            _context.Update(image);
            _context.SaveChanges();
            return image;
        }

         public async Task Delete(Image image)
        {
            _context.Remove(image);
            await _context.SaveChangesAsync();
        }

    }
}
