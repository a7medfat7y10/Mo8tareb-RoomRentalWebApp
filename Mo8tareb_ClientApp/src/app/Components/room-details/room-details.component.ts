import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-room-details',
  templateUrl: './room-details.component.html',
  styleUrls: ['./room-details.component.css']
})
export class RoomDetailsComponent {
  ID: any;
  room: any;
  constructor(myActivated: ActivatedRoute, private myService: RoomServiceService, private sanitizer: DomSanitizer){
    this.ID = myActivated.snapshot.params["id"];
  }
  ngOnInit(): void {
    this.myService.getRoomById(this.ID).subscribe({
      next:(data)=>{
        this.room = data;

        this.room.images = this.room.images.map((image:any) =>({
          id: image.id,
          imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
          }))

      },

      error:(err)=>{console.log(err)},
    })
  }
}
