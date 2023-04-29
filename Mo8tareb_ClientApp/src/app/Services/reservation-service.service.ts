import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReservationServiceService {

  private  url = "https://localhost:7188/api/Reservations/";
  private roomId: any;
  private obj: any;
  constructor(private ReservationCrud:HttpClient) { }

  DidThisUserReserveThisRoom(email: any, roomid: any) {
    this.roomId = +roomid;
    this.obj = {
        "userEmail": email,
        "roomId":this.roomId
    }
    return this.ReservationCrud.post(this.url + "DidThisUserReserveThisRoom", this.obj );
  }
  DidThisUserReserveThisRoomAndGetApprovedByOwner(email: any, roomid: any) {
    this.roomId = +roomid;
    this.obj = {
      "userEmail": email,
      "roomId":this.roomId
  }
    return this.ReservationCrud.post(this.url + "DidThisUserReserveThisRoomAndGetApprovedByOwner", this.obj );
  }
  DidThisUserReserveThisRoomAndGetRejectedByOwner(email: any, roomid: any) {
    this.roomId = +roomid;
    this.obj = {
      "userEmail": email,
      "roomId":this.roomId
  }
    return this.ReservationCrud.post(this.url + "DidThisUserReserveThisRoomAndGetRejectedByOwner", this.obj );
  }
  DidThisUserReserveThisRoomAndGetSuspendedByOwner(email: any, roomid: any) {
    this.roomId = +roomid;
    this.obj = {
      "userEmail": email,
      "roomId":this.roomId
  }
    return this.ReservationCrud.post(this.url + "DidThisUserReserveThisRoomAndGetSuspendedByOwner", this.obj );
  }

  CreateReservation(Reservation:any) {
    return this.ReservationCrud.post(this.url+"CreateReservation",Reservation);
  }


}
