using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.ServiceRepo
{
        public interface IServiceRepo : IGenericRepo<Service>
        {
            public Task<IQueryable<Service>> GetAllServicesWithRooms();

        }
}

