import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-admin-rooms',
  templateUrl: './admin-rooms.component.html',
  styleUrls: ['./admin-rooms.component.css']
})
export class AdminRoomsComponent implements OnInit {
  constructor(private myService: RoomServiceService,
    private sanitizer: DomSanitizer, private AccountService:AccountApiService,
    private myClient: HttpClient,
    private route:Router){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  rooms: any;
  roomToDelete:any;
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.myService.getAllRooms().subscribe({
      next:(data)=>{
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

  Delete(id: number) {
    console.log(id);
    this.roomToDelete = { id: id }; // initialize reviewToDelete variable with id property
    this.myClient.delete("https://localhost:7188/api/Rooms/DeleteRoom?id=" + id, { body: this.roomToDelete }).subscribe({
      next: () => {
        console.log('Room deleted successfully');
        this.route.navigateByUrl('/', { skipLocationChange: true }).then(() => {
          this.route.navigate(['/Admindashboard/rooms']);
        });
      },
      error: (err) => {
        console.error(err);
      }
    });
  }


}
