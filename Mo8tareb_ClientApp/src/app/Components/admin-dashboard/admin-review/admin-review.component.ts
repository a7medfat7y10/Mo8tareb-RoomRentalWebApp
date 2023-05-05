import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-admin-review',
  templateUrl: './admin-review.component.html',
  styleUrls: ['./admin-review.component.css']
})
export class AdminReviewComponent implements OnInit {
  constructor(private myClient: HttpClient, private sanitizer: DomSanitizer,private AccountService:AccountApiService, private route:Router){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  reviews: any;
  reviewToDelete:any;
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.myClient.get("https://localhost:7188/api/Reviews/GetAllReviewsWithUsersWithRoomsAsync").subscribe({
      next:(data:any)=>{
        console.log(data);

        this.reviews = data


      },

      error:()=>{}
    });
  }

  Delete(id: number) {
    console.log(id);
    this.reviewToDelete = { id: id }; // initialize reviewToDelete variable with id property
    this.myClient.delete("https://localhost:7188/api/Reviews/DeleteReview?id=" + id, { body: this.reviewToDelete }).subscribe({
      next: () => {
        console.log('Review deleted successfully');
        this.route.navigateByUrl('/', { skipLocationChange: true }).then(() => {
          this.route.navigate(['/Admindashboard/reviews']);
        });
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

}

