using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ImagesManagers
{
    public interface IimageManager
    {
        Task<IEnumerable<Image>> GetAllAsync();

        Task<Image> GetById(int id);

        Task<IEnumerable<Image>> GetByRoomId(int id);

        Task Add(Image image);

        Image Update (Image image);

        Task Delete(Image image);
    }
}
