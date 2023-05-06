using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.Api.Services.Email;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class OwnersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public OwnersController( IUnitOfWork unitOfWork,
                                 UserManager<AppUser> userManager,
                                 IEmailSender emailSender
                                )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        [HttpPost("ApproveReservationPayment")]
        public async Task<IActionResult> ApproveReservationPayment([FromQuery] int reservationId)
        {
            Reservation reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
           

            if (reservation is not null  && reservation.Status == ReservationStatus.Pending  )
            {
                reservation.Status = ReservationStatus.Approved;
                _unitOfWork.Reservations.Update(reservation);
                await _unitOfWork.SaveAsync();

                AppUser? user = await _userManager.FindByIdAsync(reservation.UserId!);

                // Send email to the user
                var message = new Message(new string[] { user.Email! },
                                           subject: "Mo8tareb Room Rental Web App: Reservation Approved",
                                           content: GetApprovalEmailHtml(user, reservation),
                                           attachments: null!,
                                           isHtml: true
                                          );
                await _emailSender.SendEmailAsync(message);

                return Ok();
            }
            return NotFound();
        }

        [HttpPost("RejectReservationPayment")]
        public async Task<IActionResult> RejectReservationPayment([FromQuery] int reservationId)
        {
            Reservation reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);

            if (reservation is not null && reservation.Status == ReservationStatus.Pending)
            {
                // Update the reservation status
                reservation.Status = ReservationStatus.Rejected;
                _unitOfWork.Reservations.Update(reservation);
                await _unitOfWork.SaveAsync();

                AppUser? user = await _userManager.FindByIdAsync(reservation.UserId!);


                // Send notification to the user

                var message = new Message(new string[] { user?.Email! },
                                                  subject: "Reservation Payment Rejected",
                                                  $"Hi {user?.UserName},<br/>Your reservation payment has been rejected." +
                                                  $" Please contact the room owner for more information.",
                                                  attachments: null!,
                                                  isHtml: false
                                                 );

                await _emailSender.SendEmailAsync(message);

                return Ok();
            }
            return NotFound();
        }

        private string GetApprovalEmailHtml(AppUser user, Reservation reservation)
        {
            string url = $"http://localhost:4200/ReservationApproved/{reservation.RoomId}";

            return $@"<html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    font-size: 16px;
                    color: #333;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <h2>Your reservation request has been approved! </h2>
            <p>Dear {user.FirstName},</p>
            <p>We are pleased to inform you that your reservation request for the room with ID {reservation.RoomId} has been approved by the Owner but to reserve the room you should pay first so .</p>
            <h4>P.S. - Press on this link to navigate to the payment process <a href=""{url}"">here</a>.</h4>
            <p>Mo8tareb Room Rental Web App Team</p>
            <p>If you have any questions, please do not hesitate to contact us on this Phone {reservation?.Room?.Owner?.PhoneNumber}.</p>
            <p>Best regards,</p>
        </body>
    </html>";
        }
    }
    //<p>Please note that the payment of {reservation?.Payment?.Amount} has been deducted from your account.</p>
}
