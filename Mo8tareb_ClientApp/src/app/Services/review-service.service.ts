import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserReservationDto } from '../Models/user-reservation-dto.model';

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
  submitReview(payload: any) {

    
    return fetch(this.reviewsUrl,
      {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(payload)
    })
    .then(response => {
      console.log(response); // add this line to check the response
      return response;
    });
  }

  GetAllReview(roomId:any) {


    return fetch(`https://localhost:7188/api/Reviews/GetAllReviewsOfRoom?roomId=${roomId}`,
    //   {
    //   method: 'POST',
    //   headers: {
    //     'Content-Type': 'application/json'
    //   },
    //   body: JSON.stringify()
      // }
    )
    .then(response => {
      console.log(response); // add this line to check the response
      return response;
    });
  }


}
