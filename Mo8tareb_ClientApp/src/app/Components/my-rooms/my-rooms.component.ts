import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { RoomServiceService } from 'src/app/Services/room-service.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-my-rooms',
  templateUrl: './my-rooms.component.html',
  styleUrls: ['./my-rooms.component.css']
})
export class MyRoomsComponent {

  constructor(private myService: RoomServiceService, private sanitizer: DomSanitizer,private translate: TranslateService){}
  // rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  // reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];
  rooms: any;
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.myService. getAllRoomsOfUser().subscribe({
      next:(data:any)=>{
        this.rooms = data
        console.log(data)

        this.rooms.forEach((room:any) => {
                    room.images = room.images.map((image:any) =>({
                    id: image.id,
                    imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
                  }))
        })
        console.log(this.rooms);
      },

      error: () => {
        this.rooms = [];
      }
    });
  }
  isRtl(): boolean {
    const currentLang = this.translate.currentLang;
    return currentLang === 'ar';
  }
}
