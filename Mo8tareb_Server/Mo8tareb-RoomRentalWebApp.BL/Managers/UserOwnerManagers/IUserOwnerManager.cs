using Mo8tareb_RoomRentalWebApp.BL.Dtos.AppUserOwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.UserOwnersManagers
{
    public interface IUserOwnerManager
    {
        Task<AppUserDto> GetUserByIdAsync(string userId);
        Task<OwnerReadDtosV2> GetOwnerById(string ownerId);

    }
}
