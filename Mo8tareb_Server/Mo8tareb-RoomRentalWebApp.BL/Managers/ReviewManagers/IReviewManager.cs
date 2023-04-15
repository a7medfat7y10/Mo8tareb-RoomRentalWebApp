using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReviewManagers
{
    public interface IReviewManager
    {
        Task<IQueryable<ReviewsReadDtos>> GetAllReviews();
        Task<IQueryable<ReviewsReadDtos>> GetAllReviewsWithUsersWithRoomsAsync();
        Task<ReviewsReadDtos> GetReviewById(int id);
        Task<ReviewsReadDtos?> GetDetailsById(int id);
        Task<ReviewsReadDtos> CreateReview(ReviewsReadDtos createReviewDto);
        Task<ReviewsCreateDtos?>? CreateReviewWithUsersWithRoomsAsync(ReviewsCreateDtos? createReviewDto);
        Task<ReviewsReadDtos> UpdateReview(int id, ReviewsReadDtos updateReviewDto);
        Task<ReviewsReadDtos> DeleteReview(int id);
        Task<ReviewsUpdateDtos?>? UpdateReviewAsync(ReviewsUpdateDtos review);
        Task<ReviewsToDeleteDtos?>? DeleteReviewAsync(ReviewsToDeleteDtos review);
    }
}
