using Microsoft.AspNetCore.Identity;
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
        public async Task<ReviewsCreateDtos?>? CreateReviewWithUsersWithRoomsAsync(ReviewsCreateDtos? createReviewDto)
        {

            var user = await _userManager.FindByEmailAsync(createReviewDto?.User.Email??"");
            //var roomId= _UnitOfWork.RoomRepo.getbyId(createReviewDto.Room.id) متنساش تضيف السطر دا لما تعمل Repo of room
            
                if (user == null ||createReviewDto==null)//|| roomId==null
                return null;

                Review CreatedReview = new Review()
                {
                    Comment = createReviewDto.comment,
                    Rating = createReviewDto.Rating,
                    UserId = user.Id,
                    RoomId = createReviewDto.Room.id
                };
           await _UnitOfWork.Reviews.AddAsync(CreatedReview);
           int rowsAffected=await _UnitOfWork.SaveAsync();

            return rowsAffected > 0 ?
               createReviewDto : null;
        }
        public async Task<ReviewsUpdateDtos?>? UpdateReviewAsync(ReviewsUpdateDtos review)
        {
            Review? reviewFromDatabase= _UnitOfWork.Reviews.FindByCondtion(r=> r.Id==review.id).FirstOrDefault();
            if (reviewFromDatabase == null)
                return null;

            reviewFromDatabase.Comment = review.comment;
            reviewFromDatabase.Rating = review.Rating;

            try { 
            _UnitOfWork.Reviews.Update(reviewFromDatabase);
                int rowsAffected =await _UnitOfWork.SaveAsync();
                if (rowsAffected <= 0) throw new Exception();
            }
            catch
            {
                return null;
            }
            return review;

        }
        public async Task<IQueryable<ReviewsReadDtos>> GetAllReviews()
        {
            throw new NotImplementedException();
           
        }
        public async Task<ReviewsToDeleteDtos?>? DeleteReviewAsync(ReviewsToDeleteDtos review)
        {
            Review? reviewFromDatabase = _UnitOfWork.Reviews.FindByCondtion(r => r.Id == review.id).FirstOrDefault();
            if (reviewFromDatabase == null)
                return null;

            try { 
                _UnitOfWork.Reviews.Remove(reviewFromDatabase); 
                int rowsAffected = await _UnitOfWork.SaveAsync();
                if (rowsAffected <= 0) throw new Exception();
            }
            catch { return null; }

            return review;
        }

        public Task<ReviewsReadDtos?> GetDetailsById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewsReadDtos> CreateReview(ReviewsReadDtos createReviewDto)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewsReadDtos> DeleteReview(int id)
        {
            throw new NotImplementedException();
        }

      

        public Task<ReviewsReadDtos> GetReviewById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewsReadDtos> UpdateReview(int id, ReviewsReadDtos updateReviewDto)
        {
            throw new NotImplementedException();
        }

    }
}
