﻿using FluentValidation;
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
        private readonly IReviewManager _ReviewManager;
        private readonly IValidator<CreateReviewPayload> _createReviewPayloadValidator;

        public ReviewsController(IReviewManager ReviewManager, IValidator<CreateReviewPayload> createReviewPayloadValidator)
        {
            _ReviewManager = ReviewManager;
            _createReviewPayloadValidator = createReviewPayloadValidator;
        }


        [HttpGet]
        [Route("GetAllReviewsWithUsersWithRoomsAsync")]
        public async Task<IActionResult> GetAllReviewsWithUsersWithRoomsAsync()
        {
            var lst = await _ReviewManager.GetAllReviewsWithUsersWithRoomsAsync();

            return lst.Count() == 0 ? NotFound() : Ok(lst);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewPayload payload)
        {
            // Validate payload

            var validationResult = _createReviewPayloadValidator.Validate(payload);
            if (!validationResult.IsValid)
                return BadRequest(new { StatusCode = 400, Errors = validationResult.Errors.ToDictionary(i => i.PropertyName, i => i.ErrorMessage) });

            var result = await _ReviewManager.CreateReviewAsync(payload);
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
