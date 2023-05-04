using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs;
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
        Task<IQueryable<RoomLocationsDto>> GetRoomsLocations();
        Task<IQueryable<RoomReadDto>> GetRoomsByLocation(string location);

        Task<Room> CreateRoomsAsync(RoomDto createRoomDto);
        public Task<RoomDto?>? UpdateRoomAsync(RoomDto room);
        public Task<RoomDeleteDto?>? DeleteRoomAsync(RoomDeleteDto room);

    }
}
