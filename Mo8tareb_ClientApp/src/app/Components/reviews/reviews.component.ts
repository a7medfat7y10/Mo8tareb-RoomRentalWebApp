import { Component } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { UserReservationDto } from 'src/app/Models/user-reservation-dto.model';
import { ReviewService } from 'src/app/Services/review-service.service';
import { CreateReviewPayload } from './create-review.model';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.css']
})
export class ReviewsComponent {
  userReservations: UserReservationDto[] = [];
  token: string = localStorage.getItem('M08tarebToken') ?? "";
  decodedToken: { [key: string]: any } = jwtDecode(this.token);
  userId: string = this.decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

  constructor(private reviewService: ReviewService) {
    this.reviewService.getUserReservations(this.userId)
      .subscribe((reservations: UserReservationDto[]) => {
        this.userReservations = reservations;
        console.log(this.userReservations);
      });
  }

  submitReview(reservationId: number, roomId: number | undefined, comments: string, rating: number) {
    const payload: CreateReviewPayload = {
      userId: this.userId,
      roomId: roomId,
      comments: comments,
      rating: rating,
    };
    this.reviewService.submitReview(payload).subscribe({
      next: res => {
        console.log('Review submitted successfully');
      },
      error: err => {
        console.log('Error submitting review: ', err);
      },
      complete: () => {
        console.log('Subscription complete');
      }
    });
  }

}
