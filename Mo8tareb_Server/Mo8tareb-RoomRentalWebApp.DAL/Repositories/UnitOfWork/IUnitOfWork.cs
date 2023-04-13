using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        //IDevelopersRepo Developers { get; }
        //ITicketsRepo Tickets { get; }
        //IDepartmentsRepo Departments { get; }
        Task<int> SaveAsync();
    }
}
