import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RoomServiceService {

  private readonly url = "https://localhost:7188/api/Rooms/GetAllRooms";
  constructor(private readonly myClient: HttpClient) { }
  getAllRooms(){
    return this.myClient.get(this.url);
  }

  getRoomById(id: any){
    return this.myClient.get("https://localhost:7188/api/Rooms/GetRoomById?id=" + id)
  }

  AddNewRoom(room: any){
    return this.myClient.post("https://localhost:7188/api/Rooms/CreateRoom", room);
  }

  UpdateRoomByID(id: any, room:any){
    return this.myClient.put(this.url + "/" + id, room);
  }

  DeleteRoomByID(id: any){
    return this.myClient.delete(this.url + "/" + id);
  }
}
