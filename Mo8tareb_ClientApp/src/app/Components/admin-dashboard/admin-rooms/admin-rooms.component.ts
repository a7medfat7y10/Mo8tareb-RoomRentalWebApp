import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-admin-rooms',
  templateUrl: './admin-rooms.component.html',
  styleUrls: ['./admin-rooms.component.css']
})
export class AdminRoomsComponent implements OnInit {
  constructor(private myService: RoomServiceService, private sanitizer: DomSanitizer){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  rooms: any;
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
        console.log(this.rooms);
      },

      error:()=>{}
    });
  }
}
