using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReservationRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.RoomRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ServiceRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        public IReviewRepo Reviews { get; }
        public IReservationRepo Reservations { get; }
        public IServiceRepo Services { get; }
        public IRoomRepo Rooms { get; }
        Task<int> SaveAsync();
    }
}
