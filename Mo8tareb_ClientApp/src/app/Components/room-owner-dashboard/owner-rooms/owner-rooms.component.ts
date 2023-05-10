import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-owner-rooms',
  templateUrl: './owner-rooms.component.html',
  styleUrls: ['./owner-rooms.component.css']
})
export class OwnerRoomsComponent implements OnInit {
  constructor(private myService: RoomServiceService, private sanitizer: DomSanitizer, private AccountService:AccountApiService){}
  rooms: any;
  ngOnInit(): void {
    this.myService.getAllRooms().subscribe({
      next:(data:any)=>{
        this.rooms = data;

        const ownerIds = this.rooms.map((room:any) => room.ownerId);

        forkJoin(ownerIds.map((ownerId:any) => this.AccountService.GetUserById(ownerId))).subscribe({
          next: (userList: any) => {
            this.rooms.forEach((room: any, index: number) => {
              room.useremail = userList[index].email;
            });

            const userRooms = this.rooms.filter((room:any) => this.AccountService.GetEmail() === room.useremail);

            this.rooms = userRooms;

            this.rooms.forEach((room:any) => {
              room.images = room.images.map((image:any) =>({
                id: image.id,
                imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
              }))
            });

            console.log(this.rooms);
          },
          error: (err) => {
            console.log(err);
          }
        });
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
}
