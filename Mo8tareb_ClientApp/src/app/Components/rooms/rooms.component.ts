import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent {

  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];

  // constructor(private http: HttpClient, private sanitizer: DomSanitizer) { }

  // ngOnInit() {
  //   this.getRooms();
  // }

  // getRooms() {
  //   const apiUrl = 'https://localhost:7188/GetAllRooms';
  //   this.http.get<any>(apiUrl).subscribe(
  //     (data) => {
  //       data.forEach((room:any) => {
  //         this.rooms.push(room);
  //       });
  //       this.rooms.forEach((room:any) => {
  //         room.images = room.images.map((image:any) =>({
  //           id: image.id,
  //           imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
  //         }))
  //       })
  //     },
  //     error => {
  //       console.log(error);
  //     }
  //   );

  // }

  constructor(private myService: RoomServiceService, private sanitizer: DomSanitizer, private AccountService:AccountApiService){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  rooms: any;
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.myService.getAllRooms().subscribe({
      next:(data:any)=>{

        console.log(data)
        const userRooms = data.filter((room:any)=>{
          return room.images.length !=0 && room.isReserved == false
        });

        this.rooms = userRooms;

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
