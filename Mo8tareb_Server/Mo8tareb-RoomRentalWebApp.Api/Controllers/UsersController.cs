using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.AppUserOwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.UserOwnersManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Constants;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System.Linq;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IUserOwnerManager _userOwnerManager;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(ApplicationDbContext context,IUserOwnerManager userOwnerManager, UserManager<AppUser> userManager)
        {
            this.context = context;
            _userOwnerManager = userOwnerManager;
            _userManager = userManager;
        }
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("GetUserDetailsById")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            AppUserDto? user = await _userOwnerManager.GetUserByIdAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);


        }


        [HttpGet]
        [Route("AssignRoleOwnerToUserAsync")]
        public async Task<IActionResult> AssignRoleOwnerToUserAsync([FromQuery]string Email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(Email);

            if (user is null)
            {
                return NotFound("Email Does not Exist");
            }

            try
            {
                Console.WriteLine("--------------------------------------------------------------");
                Owner owner = new Owner()
                {
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEnd = user.LockoutEnd,
                    SecurityStamp = user.SecurityStamp,
                    ConcurrencyStamp = user.ConcurrencyStamp,
                    AccessFailedCount = user.AccessFailedCount,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    //Id = user.Id,
                    LastName = user.LastName,
                    NormalizedEmail = user.NormalizedEmail,
                    NormalizedUserName = user.NormalizedUserName,
                    PasswordHash = user.PasswordHash,
                 
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    Reservations = user.Reservations,
                    Reviews = user.Reviews,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    UserName = user.UserName,
                    Rooms = null
                };
                // Delete all reservations associated with the user
                var reservationIds = await context.Reservations.Where(r => r.UserId == user.Id)
                    .Select(r => r.Id)
                    .ToListAsync();

                var payments = await context.payments
                    .Where(p => reservationIds.Contains( (int)p.ReservationId! ))
                    .ToListAsync();

                context.payments.RemoveRange(payments);

                var reservations = await context.Reservations.Where(r => r.UserId == user.Id).ToListAsync();
                context.Reservations.RemoveRange(reservations);

                var Reviews = await context.Reviews.Where(r => r.UserId == user.Id).ToListAsync();
                context.Reviews.RemoveRange(Reviews);


                context.SaveChanges();
                // Create new owner and delete old user
                await _userManager.DeleteAsync(user);
                await _userManager.CreateAsync(owner);
                await _userManager.AddToRoleAsync(owner, Authorization.Owner);
            }
            catch
            {
                return BadRequest("Error to create owner");
            }

            return Ok();
        }

    }

}
