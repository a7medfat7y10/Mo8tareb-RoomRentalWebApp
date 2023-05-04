using Microsoft.AspNetCore.Identity;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mo8tareb_RoomRentalWebApp.BL.Managers.UserOwnersManagers;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.AppUserOwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;

namespace Mo8tareb_UserOwnerRentalWebApp.BL.Managers.UserOwnerManagers
{
    public class UserOwnerManager : IUserOwnerManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public UserOwnerManager(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;

        }

        public async Task<AppUserDto> GetUserByIdAsync(string userId)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return null!;
            }

         
            List<ReservationsReadDtosV2>? reservations = user?.Reservations?.Select(r => new ReservationsReadDtosV2
            {
                Id = r.Id,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status,
                User = new userReadDtosV2
                {
                    Email = r?.User?.Email,
                    firstName = r?.User.FirstName,
                    lastName = r?.User.LastName
                },
                Room = new RoomReadDtosV2
                {
                    id = r.Room.Id,
                    RoomType = r.Room.RoomType
                }
            }).ToList();

            List<ReviewsReadDtosV2>? reviews = user?.Reviews?.Select(r => new ReviewsReadDtosV2
            {
                id = r.Id,
                comment = r.Comment,
                Rating = r.Rating,
                User = new userReadDtosV2
                {
                    Email = r.User.Email,
                    firstName = r.User.FirstName,
                    lastName = r.User.LastName
                },
                Room = new RoomReadDtosV2
                {
                    id = r.Room.Id,
                    RoomType = r.Room.RoomType,
                    Owner = new OwnerReadDtosV2
                    {
                        Email = r.User.Email,
                        firstName = r.User.FirstName,
                        lastName = r.User.LastName
                    }
                }
            }).ToList();

            AppUserDto? appUserDto = new AppUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Reservations = reservations,
                Reviews = reviews,
              
            };

            return appUserDto;
        }

        public async Task<OwnerReadDtosV2> GetOwnerById(string ownerId)
        {
            Owner? owner = await _userManager.FindByIdAsync(ownerId) as Owner;

            if (owner is null)
            {
                return null!;
            }


            return new OwnerReadDtosV2
            {
                Email = owner.Email,
                firstName = owner.FirstName,
                lastName = owner.LastName,
                PhoneNumber = owner.PhoneNumber
            };
        }


    }
}
