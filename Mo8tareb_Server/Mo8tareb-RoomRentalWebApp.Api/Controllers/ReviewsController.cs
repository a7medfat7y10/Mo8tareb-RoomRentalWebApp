using FluentValidation;
using Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReviewManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using Newtonsoft.Json;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DAL.Context.ApplicationDbContext context;
        private readonly IReviewManager _ReviewManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<CreateReviewPayload> _createReviewPayloadValidator;

       

        public ReviewsController(ApplicationDbContext context,UserManager<AppUser> userManager,IReviewManager ReviewManager, IValidator<CreateReviewPayload> createReviewPayloadValidator)
        {
            this.context = context;
            _ReviewManager = ReviewManager;
            _createReviewPayloadValidator = createReviewPayloadValidator;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("GetAllReviewsWithUsersWithRoomsAsync")]
        public async Task<IActionResult> GetAllReviewsWithUsersWithRoomsAsync()
        {
            var lst = await _ReviewManager.GetAllReviewsWithUsersWithRoomsAsync();

            return lst.Count() == 0 ? NotFound() : Ok(lst);
        }

        [HttpGet]
        [Route("GetAllReviewsOfRoom")]
        public async Task<IActionResult> GetAllReviewsOfRoom(int roomId)
        {

           var reviewLst= context.Reviews.Include(r => r.Room).Include(r => r.User).Where(r=>r.RoomId==roomId).ToList();
            List<ReviewsReadDtosV22> newReviewList = new();
            ReviewsReadDtosV22 obj;
            foreach (var review in reviewLst)
            {
               var user= await _userManager.FindByIdAsync(review.UserId!);

                obj = new(review.Id, review.Comment??"", review.Rating, new BL.Dtos.UserDtos.userReadDtos(user.Email, user.FirstName, user.LastName));
                newReviewList.Add(obj);

            }
            return newReviewList.Count() == 0 ? NotFound() : Ok(newReviewList);
        }


        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewPayload payload)
        {
            // Validate payload

            var validationResult = _createReviewPayloadValidator.Validate(payload);
            if (!validationResult.IsValid)
                return BadRequest(new { StatusCode = 400, Errors = validationResult.Errors.ToDictionary(i => i.PropertyName, i => i.ErrorMessage) });

            var result = await _ReviewManager.CreateReviewWithUsersWithRoomsAsync(payload);

            var responseData = new
            {
                result = result
            };
            var json = JsonConvert.SerializeObject(responseData);
            return result is null ? BadRequest("Error") : Ok("Success");
        }


        [HttpPut]
        [Route("UpdateReview")]
        public async Task<IActionResult> UpdateReview(int id, ReviewsUpdateDtos review)
        {
            if (review == null || id != review.id)
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
