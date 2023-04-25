import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserReservationDto } from '../Models/user-reservation-dto.model';
import { CreateReviewPayload } from '../Components/reviews/create-review.model';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private readonly reservationsUrl = "https://localhost:7188/api/Reservations";
  private readonly reviewsUrl = "https://localhost:7188/api/Reviews";
  constructor(private readonly httpClient: HttpClient) { }

  getUserReservations(userId: string): Observable<UserReservationDto[]> {
    return this.httpClient.get<UserReservationDto[]>(`${this.reservationsUrl}/${userId}`);
  }
  submitReview(payload : CreateReviewPayload){
    return this.httpClient.post(`${this.reviewsUrl}`, payload);
  }
}
