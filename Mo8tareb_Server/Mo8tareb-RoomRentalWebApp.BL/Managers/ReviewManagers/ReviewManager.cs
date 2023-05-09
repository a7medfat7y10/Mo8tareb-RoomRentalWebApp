using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;

using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System.Linq;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReviewManagers
{
    public class ReviewManager : IReviewManager
    {
        public readonly IUnitOfWork _UnitOfWork;
        public readonly UserManager<AppUser> _userManager;
        public ReviewManager(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IQueryable<ReviewsReadDtos>> GetAllReviewsWithUsersWithRoomsAsync()
        {
            var ReviewsList = await _UnitOfWork.Reviews.GetAllReviewsWithUsersWithRoomsRepoFuncAsync();
            var ReviewsDtos = ReviewsList.Select(r =>
            new ReviewsReadDtos
            (
                 r.Id,
                 r.Comment!,
                 r.Rating,
                 new userReadDtos(r.User!.Email!, r.User.FirstName, r.User.LastName),
                 new RoomReadDtos(r.Room!.Id, r.Room.RoomType, new OwnerReadDtos(r.Room!.Owner!.Email!, r.Room.Owner.FirstName, r.Room.Owner.LastName))
            ));
            return ReviewsDtos;
        }
        public async Task<CreateReviewPayload?>? CreateReviewWithUsersWithRoomsAsync(CreateReviewPayload? createReviewDto)
        {

            var user = await _userManager.FindByEmailAsync(createReviewDto?.UserEmail ?? "");
            //var roomId= _UnitOfWork.RoomRepo.getbyId(createReviewDto.Room.id) متنساش تضيف السطر دا لما تعمل Repo of room

            if (user == null || createReviewDto == null)//|| roomId==null
                return null;

            // Check if room exists and user made a reservation in that room

            var room = await _UnitOfWork.Rooms.GetByIdAsync(createReviewDto.RoomId);

            if (room is null)
                return null;

            //var reservation = await _UnitOfWork.Reservations
            //    .FindByCondtion(i => i.UserId == user.Id && i.RoomId == room.Id)
            //    .FirstOrDefaultAsync();

            //if (reservation is null) 
            //    return null;


            Review CreatedReview = new Review()
            {
                Comment = createReviewDto.Comments,
                Rating = createReviewDto.Rating,
                UserId = user.Id,
                RoomId = createReviewDto.RoomId
            };
            await _UnitOfWork.Reviews.AddAsync(CreatedReview);
            int rowsAffected = await _UnitOfWork.SaveAsync();

            return rowsAffected > 0 ?
               createReviewDto : null;
        }
        public async Task<ReviewsUpdateDtos?>? UpdateReviewAsync(ReviewsUpdateDtos review)
        {
            Review? reviewFromDatabase = _UnitOfWork.Reviews.FindByCondtion(r => r.Id == review.id).FirstOrDefault();
            if (reviewFromDatabase == null)
                return null;

            reviewFromDatabase.Comment = review.comment;
            reviewFromDatabase.Rating = review.Rating;

            try
            {
                _UnitOfWork.Reviews.Update(reviewFromDatabase);
                int rowsAffected = await _UnitOfWork.SaveAsync();
                if (rowsAffected <= 0) throw new Exception();
            }
            catch
            {
                return null;
            }
            return review;

        }
        public async Task<ReviewsToDeleteDtos?>? DeleteReviewAsync(ReviewsToDeleteDtos review)
        {
            Review? reviewFromDatabase = _UnitOfWork.Reviews.FindByCondtion(r => r.Id == review.id).FirstOrDefault();
            if (reviewFromDatabase == null)
                return null;

            try
            {
                _UnitOfWork.Reviews.Remove(reviewFromDatabase);
                int rowsAffected = await _UnitOfWork.SaveAsync();
                if (rowsAffected <= 0) throw new Exception();
            }
            catch { return null; }

            return review;
        }

        public async Task<dynamic> CreateReviewAsync(CreateReviewPayload payload)
        {

            var user = await _userManager.FindByIdAsync(payload.UserEmail ?? "");
            //var roomId= _UnitOfWork.RoomRepo.getbyId(createReviewDto.Room.id) متنساش تضيف السطر دا لما تعمل Repo of room

            if (user == null)//|| roomId==null
                return null;

            // Check if room exists and user made a reservation in that room

            var room = await _UnitOfWork.Rooms.GetByIdAsync(payload.RoomId);

            if (room is null)
                return null;

            var reservation = await _UnitOfWork.Reservations
                .FindByCondtion(i => i.UserId == user.Id && i.RoomId == room.Id)
                .FirstOrDefaultAsync();

            if (reservation is null)
                return null;


            Review CreatedReview = new Review()
            {
                Comment = payload.Comments,
                Rating = payload.Rating,
                UserId = user.Id,
                RoomId = payload.RoomId
            };
            await _UnitOfWork.Reviews.AddAsync(CreatedReview);
            int rowsAffected = await _UnitOfWork.SaveAsync();

            return rowsAffected > 0 ?
               new {Message = "Success"} : null;
        }
    }
}
