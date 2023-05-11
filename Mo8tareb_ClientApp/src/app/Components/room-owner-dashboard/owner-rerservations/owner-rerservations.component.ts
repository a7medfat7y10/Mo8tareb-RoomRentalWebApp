import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AccountApiService } from 'src/app/Services/account-api.service';

@Component({
  selector: 'app-owner-rerservations',
  templateUrl: './owner-rerservations.component.html',
  styleUrls: ['./owner-rerservations.component.css']
})
export class OwnerRerservationsComponent implements OnInit {
  constructor(private myClient: HttpClient, private sanitizer: DomSanitizer, private AccountService: AccountApiService){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  reservations: any;
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.myClient.get("https://localhost:7188/api/Reservations/GetAllReservationsWithUsersWithRoomsAsync").subscribe({
      next:(data:any)=>{
        const userReservations = data.filter((res:any)=>{
          return this.AccountService.GetEmail() == res.room.owner.email;
        });

        this.reservations = userReservations;
        console.log(this.reservations)
      },

      error:()=>{}
    });
  }
  Approve(id:any){
    this.myClient.post("https://localhost:7188/api/Owners/ApproveReservationPayment?reservationId="+ id, {
      id: id,
      status: 1 // set status to "approved" (1)
    }).subscribe({
      next: () => {
        console.log('Reservation approved successfully');
        window.alert("Reservation approved successfully ");
        // refresh reservations
        this.myClient.get("https://localhost:7188/api/Reservations/GetAllReservationsWithUsersWithRoomsAsync").subscribe({
          next:(data:any)=>{

            this.reservations = data.filter((res:any)=>{
              return this.AccountService.GetEmail() == res.room.owner.email;
            });
          },
          error:()=>{}
        });
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  Reject(id:any){
    this.myClient.post("https://localhost:7188/api/Owners/RejectReservationPayment?reservationId="+ id, {
      id: id,
      status: 2 // set status to "rejected" (2)
    }).subscribe({
      next: () => {
        console.log('Reservation rejected successfully');
        // refresh reservations
        this.myClient.get("https://localhost:7188/api/Reservations/GetAllReservationsWithUsersWithRoomsAsync").subscribe({
          next:(data:any)=>{
            window.alert("Reservation rejected successfully ")
            this.reservations = data.filter((res:any)=>{

              return this.AccountService.GetEmail() == res.room.owner.email;
            });
          },
          error:()=>{}
        });
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
