using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.Api.Services.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
