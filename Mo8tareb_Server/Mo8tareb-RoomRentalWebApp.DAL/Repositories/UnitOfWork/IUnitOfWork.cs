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
        public IReviewRepo Reviews { get; }
        Task<int> SaveAsync();
    }
}
