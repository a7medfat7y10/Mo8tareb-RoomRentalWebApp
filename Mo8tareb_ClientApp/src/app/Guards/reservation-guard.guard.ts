import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Route, RouterStateSnapshot, Routes, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountApiService } from '../Services/account-api.service';
import { ReservationServiceService } from '../Services/reservation-service.service';
import { RoomServiceService } from '../Services/room-service.service';

@Injectable({
  providedIn: 'root'
})
export class ReservationGuardGuard implements CanActivate {
  roomId: any = 0;
constructor(private ReservationService:ReservationServiceService, private myActivate: ActivatedRoute, private roomCrud: RoomServiceService,
  private AccountService: AccountApiService) {
    this.roomId = myActivate.snapshot.params["id"];

  }

  isReservationApprovedByOwnerFlag: Boolean = false;
  isReservationRejectedByOwnerFlag: Boolean = false;
  isReservationSuspendedByOwnerFlag: Boolean = false;
  DidUserReserveTHisRoomFlag: Boolean = false;


  isReservationApprovedByOwner() {
    this.ReservationService.DidThisUserReserveThisRoomAndGetApprovedByOwner(this.AccountService.GetEmail(), this.roomId).subscribe({
      next: (data:any) => {
        console.log(data);
        this.isReservationApprovedByOwnerFlag = data;
        return this.isReservationApprovedByOwnerFlag;
      },
      error: (err) => {
        this.isReservationApprovedByOwnerFlag = false;
        return this.isReservationApprovedByOwnerFlag;

      }
    });
  }
  DidUserReserveTHisRoom():any {
    this.ReservationService.DidThisUserReserveThisRoom(this.AccountService.GetEmail(), this.roomId).subscribe({
      next: (data:any) => {
        console.log(data)
        this.DidUserReserveTHisRoomFlag = data;
        return this.DidUserReserveTHisRoomFlag ;
      },
      error: (err) => {
        this.DidUserReserveTHisRoomFlag = false;
        return this.DidUserReserveTHisRoomFlag ;

      }
    });
  }
  isReservationRejectedByOwner() {
    this.ReservationService.DidThisUserReserveThisRoomAndGetRejectedByOwner(this.AccountService.GetEmail(), this.roomId).subscribe({
      next: (data:any) => {
        console.log(data)
        this.isReservationRejectedByOwnerFlag = data;
        return this.isReservationRejectedByOwnerFlag;
      },
      error: (err) => {
        this.isReservationRejectedByOwnerFlag = false;
        return this.isReservationRejectedByOwnerFlag;
      }
    });
  }
 isReservationSuspendedByOwner() {
    this.ReservationService.DidThisUserReserveThisRoomAndGetSuspendedByOwner(this.AccountService.GetEmail(), this.roomId).subscribe({
      next: (data:any) => {
        console.log(data)
        this.isReservationSuspendedByOwnerFlag = data;
        return this.isReservationRejectedByOwnerFlag;
      },
      error: (err) => {
        this.isReservationSuspendedByOwnerFlag = false;
        return this.isReservationRejectedByOwnerFlag;
      }
    });
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (this.DidUserReserveTHisRoom() == false)
      window.location.href = `ReserveRoom/${this.roomId}`;
    else if (this.DidUserReserveTHisRoom() == true && this.isReservationSuspendedByOwnerFlag==true)
        window.location.href = `ReservationSuspended/${this.roomId}`;
    else if (this.DidUserReserveTHisRoom() == true && this.isReservationApprovedByOwnerFlag==true)
      window.location.href = `ReservationApproved/${this.roomId}`;
    else if (this.DidUserReserveTHisRoom() == true && this.isReservationRejectedByOwnerFlag==true)
      window.location.href = `ReservationRejected/${this.roomId}`;

    return true;
  }

}
