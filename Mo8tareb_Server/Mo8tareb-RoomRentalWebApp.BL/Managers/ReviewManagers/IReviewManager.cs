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
        Task<IQueryable<ReviewsReadDtos>> GetAllReviewsWithUsersWithRoomsAsync();
        Task<CreateReviewPayload?>? CreateReviewWithUsersWithRoomsAsync(CreateReviewPayload? createReviewDto);
        Task<ReviewsUpdateDtos?>? UpdateReviewAsync(ReviewsUpdateDtos review);
        Task<ReviewsToDeleteDtos?>? DeleteReviewAsync(ReviewsToDeleteDtos review);
        Task<dynamic> CreateReviewAsync(CreateReviewPayload payload);
    }
}
