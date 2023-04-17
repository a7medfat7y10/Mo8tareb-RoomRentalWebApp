using Mo8tareb_RoomRentalWebApp.DAL.Models;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReservationRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo;
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
        IReviewRepo Reviews { get; }
        IReservationRepo ReservationRepo { get; }
        Task<int> SaveAsync();
    }
}
