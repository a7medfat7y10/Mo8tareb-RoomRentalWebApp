import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountApiService } from './account-api.service';

@Injectable({
  providedIn: 'root'
})
export class RoomServiceService {

  private readonly url = "https://localhost:7188/api/Rooms/GetAllRooms";
  constructor(private readonly myClient: HttpClient,private account:AccountApiService) { }
  getAllRooms(){
    return this.myClient.get(this.url);
  }
  getAllRoomsOfUser(){
    return this.myClient.get("https://localhost:7188/api/Rooms/GetAllRoomsOfUser?Email="+this.account.GetEmail());
  }

  getRoomById(id: any){
    return this.myClient.get("https://localhost:7188/api/Rooms/GetRoomById?id=" + id)
  }

  AddNewRoom(room: any){
    return this.myClient.post("https://localhost:7188/api/Rooms/CreateRoom", room);
  }

  UpdateRoomByID(id: any, room:any){
    return this.myClient.put("https://localhost:7188/api/Rooms/UpdateRoom?id="+ id, room);
  }

  DeleteRoomByID(id: any){
    return this.myClient.delete(this.url + "/" + id);
  }
}
