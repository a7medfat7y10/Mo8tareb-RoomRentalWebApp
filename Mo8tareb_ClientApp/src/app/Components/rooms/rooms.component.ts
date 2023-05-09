import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})

export class RoomsComponent {

  constructor(private myService: RoomServiceService, private sanitizer: DomSanitizer, private AccountService:AccountApiService, private myClient:HttpClient, private translate:TranslateService){}

  rooms: any;
  locations: any;
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
         // Get distinct locations using a Set
        const locationsSet = new Set<string>(this.rooms.map((room: any) => room.location.toLowerCase()));
        this.locations = Array.from(locationsSet);

        console.log(this.rooms);
        console.log(this.locations);
      },

      error:()=>{}
    });
  }


  location: any = "All";
  onChange(location: any) {
    console.log(location);
    if(location == "All")
    {
        this.myService.getAllRooms().subscribe({
        next:(data:any)=>{
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
        },
        error:()=>{}
      });
    }
    else{

      this.myService.getAllRooms().subscribe({
        next:(data:any)=>{

          console.log(data)
          const userRooms = data.filter((room:any)=>{
            return room.images.length !=0 && room.isReserved == false && room.location.toLowerCase() == location.toLowerCase();
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


isRtl(): boolean {
  const currentLang = this.translate.currentLang;
  return currentLang === 'ar';
}

}




