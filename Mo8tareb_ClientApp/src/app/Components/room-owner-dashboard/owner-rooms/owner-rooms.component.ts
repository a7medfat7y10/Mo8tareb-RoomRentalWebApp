import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-owner-rooms',
  templateUrl: './owner-rooms.component.html',
  styleUrls: ['./owner-rooms.component.css']
})
export class OwnerRoomsComponent implements OnInit {
  constructor(private myService: RoomServiceService, private sanitizer: DomSanitizer, private AccountService:AccountApiService){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  rooms: any;
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.myService.getAllRooms().subscribe({
      next:(data:any)=>{
        this.rooms = data
        this.rooms.forEach((room:any) => {
                    room.images = room.images.map((image:any) =>({
                    id: image.id,
                    imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
                  }))
        })

        this.rooms.forEach((room:any)=>{
          this.AccountService.GetUserById(room.ownerId).subscribe({
            next: (data: any) => {
              room.useremail = data.email;
          },
          error: (err) =>{
            console.log(err);
          }
        });
        })

        console.log(this.rooms);
      },

      error:()=>{}
    });
  }
}
