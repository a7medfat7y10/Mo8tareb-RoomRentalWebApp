import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent  implements OnInit {
  constructor(private myClient: HttpClient, private sanitizer: DomSanitizer){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  reservations: any;
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.myClient.get("https://localhost:7188/api/Reservations/GetAllReservationsWithUsersWithRoomsAsync").subscribe({
      next:(data:any)=>{
        this.reservations = data
      },

      error:()=>{}
    });
  }
}

