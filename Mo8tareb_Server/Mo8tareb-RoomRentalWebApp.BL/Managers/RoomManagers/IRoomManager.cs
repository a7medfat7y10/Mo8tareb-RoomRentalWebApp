using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers
{
    public interface IRoomManager
    {
        Task<IQueryable<RoomReadDto>> GetAllRoomsWithDetails();
        public Task<RoomReadDto> GetRoomWithDetails(int id);

        Task<Room> CreateRoomsAsync(RoomCreateDto createRoomDto);
        public Task<RoomUpdateDto?>? UpdateRoomAsync(RoomUpdateDto room);
        public Task<RoomDeleteDto?>? DeleteRoomAsync(RoomDeleteDto room);

    }
}
