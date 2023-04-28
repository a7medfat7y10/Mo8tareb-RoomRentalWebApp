import { Component } from '@angular/core';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-owner-create-room',
  templateUrl: './owner-create-room.component.html',
  styleUrls: ['./owner-create-room.component.css']
})
export class OwnerCreateRoomComponent {

  constructor(public myService:RoomServiceService){

  }
  Add(location:any, price:any, roomType:any) {
    let newroom = {location, price, roomType, ownerId: "af3ece09-cdb3-4ee3-a968-dec16ac58262"};
    this.myService.AddNewRoom(newroom).subscribe((data)=>{
      console.log(data);
    });

  }
}
