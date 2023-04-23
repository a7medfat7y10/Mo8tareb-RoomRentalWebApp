using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReviewManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Context;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        public readonly IReviewManager _ReviewManager;

        public ReviewsController(IReviewManager ReviewManager)
        {
            _ReviewManager = ReviewManager;
        }


        [HttpGet]
        [Route("GetAllReviewsWithUsersWithRoomsAsync")]
        public async Task<IActionResult> GetAllReviewsWithUsersWithRoomsAsync()
        {
           var lst= await _ReviewManager.GetAllReviewsWithUsersWithRoomsAsync();

            return lst.Count()==0?NotFound():Ok(lst);
        }

        [HttpPost]
        [Route("CreateReview")]
        public async Task<IActionResult> CreateReview(ReviewsCreateDtos review)
        {
            if (review == null || !ModelState.IsValid)
                return BadRequest("Please send a Valid data to create !!");

            ReviewsCreateDtos? objectCreated = await _ReviewManager?.CreateReviewWithUsersWithRoomsAsync(review)!;

            return objectCreated != null ? Ok("Review created Succssfuly !") : BadRequest("Could not create Review due to the inValid data you sent :(");
        }


        [HttpPut]
        [Route("UpdateReview")]
        public async Task<IActionResult> UpdateReview(int id,ReviewsUpdateDtos review)
        {
            if (review == null || id !=review.id)
                return BadRequest("Please send valid Data to Update !!");

            ReviewsUpdateDtos? objectUpdated = await _ReviewManager.UpdateReviewAsync(review)!;

            return objectUpdated != null ? Ok("Review Updated Succssfuly !") : BadRequest("Could not Update Review due to the inValid data you sent  :(");
        }

        [HttpDelete]
        [Route("DeleteReview")]
        public async Task<IActionResult> DeleteReview(int id, ReviewsToDeleteDtos review)
        {
            if (review == null || id != review.id)
                return BadRequest("Please send valid Data to Update !!");

            ReviewsToDeleteDtos? objectUpdated = await _ReviewManager.DeleteReviewAsync(review)!;

            return objectUpdated != null ? Ok("Review Deleted Succssfuly !") : BadRequest("Could not Deleted Review due to the inValid data you sent  :(");
        }


    }
}
