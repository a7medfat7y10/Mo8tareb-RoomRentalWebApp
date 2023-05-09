import { Component, OnInit } from '@angular/core';
import { AccountApiService } from '../../Services/account-api.service';
import { ActivatedRoute, Route, Router, RouterLink, Routes } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent   {

  constructor(private myService: RoomServiceService, private sanitizer: DomSanitizer, private AccountService:AccountApiService, private myClient:HttpClient, private translate:TranslateService){}

  rooms: any;
  locations: any;
  ngOnInit(): void {
    this.myService.getAllRooms().subscribe({
      next: (data: any) => {
        console.log(data);

        // Filter rooms based on availability of images and isReserved property
        const userRooms = data.filter((room: any) => {
          return room.images.length !== 0 && room.isReserved === false;
        });

        // Randomly select three rooms
        this.rooms = this.getRandomRooms(userRooms, 3);

        // Sanitize image URLs
        this.rooms.forEach((room: any) => {
          room.images = room.images.map((image: any) => ({
            id: image.id,
            imageUrl: this.sanitizer.bypassSecurityTrustUrl(
              'data:image/png;base64,' + image.imageUrl
            ),
          }));
        });

        // Get distinct locations using a Set
        const locationsSet = new Set<string>(
          this.rooms.map((room: any) => room.location.toLowerCase())
        );
        this.locations = Array.from(locationsSet);

        console.log(this.rooms);
        console.log(this.locations);
      },

      error: () => {},
    });
  }

  /**
   * Returns a randomly selected subset of the given array with the specified length.
   */
  private getRandomRooms(arr: any[], len: number): any[] {
    const shuffled = arr.sort(() => 0.5 - Math.random());
    return shuffled.slice(0, len);
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

