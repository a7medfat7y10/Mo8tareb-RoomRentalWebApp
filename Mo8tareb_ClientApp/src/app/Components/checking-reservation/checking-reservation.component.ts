import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { ReservationServiceService } from 'src/app/Services/reservation-service.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-checking-reservation',
  templateUrl: './checking-reservation.component.html',
  styleUrls: ['./checking-reservation.component.css']
})
export class CheckingReservationComponent implements OnInit {
  roomId: any = 0;


  constructor(private ReservationService: ReservationServiceService, private myActivate: ActivatedRoute, private roomCrud: RoomServiceService,
    private AccountService: AccountApiService) {
      this.roomId = myActivate.snapshot.params["id"];
    }
  ngOnInit(): void {
    this.DidUserReserveTHisRoom();
  }


    isReservationApprovedByOwnerFlag: Boolean = false;
    isReservationRejectedByOwnerFlag: Boolean = false;
    isReservationSuspendedByOwnerFlag: Boolean = false;
    DidUserReserveTHisRoomFlag: Boolean = false;

  DidUserReserveTHisRoom(): any {
      console.log(this.AccountService.GetEmail(),this.roomId)
      this.ReservationService.DidThisUserReserveThisRoom(this.AccountService.GetEmail(), this.roomId).subscribe({
        next: (data:any) => {
          this.DidUserReserveTHisRoomFlag = data;
          console.log(this.DidUserReserveTHisRoomFlag)
          {
          if (this.DidUserReserveTHisRoomFlag == false)
          {
            console.log("this.DidUserReserveTHisRoom() == false")
            window.location.href = `ReserveRoom/${this.roomId}`;
          }
          else
          {
            {

              this.ReservationService.DidThisUserReserveThisRoomAndGetSuspendedByOwner(this.AccountService.GetEmail(), this.roomId).subscribe({
                next: (data:any) => {
                  console.log(data)
                  this.isReservationSuspendedByOwnerFlag = data;

                   if (this.DidUserReserveTHisRoomFlag  == true && this.isReservationSuspendedByOwnerFlag == true) {
                    console.log("this.DidUserReserveTHisRoom() == true && this.isReservationSuspendedByOwnerFlag == true")
                      window.location.href = `ReservationSuspended/${this.roomId}`;
                   }
                   else {

                    this.ReservationService.DidThisUserReserveThisRoomAndGetApprovedByOwner(this.AccountService.GetEmail(), this.roomId).subscribe({
                      next: (data:any) => {
                        console.log(data);
                        this.isReservationApprovedByOwnerFlag = data;
                         if (this.DidUserReserveTHisRoomFlag  == true && this.isReservationApprovedByOwnerFlag == true) {
                          console.log("this.DidUserReserveTHisRoom() == true && this.isReservationApprovedByOwnerFlag == true")
                            window.location.href = `ReservationApproved/${this.roomId}`;
                         }
                         else {
                          this.ReservationService.DidThisUserReserveThisRoomAndGetRejectedByOwner(this.AccountService.GetEmail(), this.roomId).subscribe({
                            next: (data:any) => {
                              console.log(data)
                              this.isReservationRejectedByOwnerFlag = data;
                               if (this.DidUserReserveTHisRoomFlag  == true && this.isReservationRejectedByOwnerFlag == true) {
                                console.log("this.DidUserReserveTHisRoom() == true && this.isReservationRejectedByOwnerFlag == true")
                                  window.location.href = `ReservationRejected/${this.roomId}`;
                               }
                               else {
                                window.location.href ="/erro404"
                              }
                            },
                            error: (err) => {
                              this.isReservationRejectedByOwnerFlag = false;
                              window.location.href ="/erro404"
                            }
                          });
                        }
                        },
                      error: (err) => {
                        this.isReservationApprovedByOwnerFlag = false;
                        window.location.href ="/erro404"
                      }
                    });
                  }
                },
                error: (err) => {
                  this.isReservationSuspendedByOwnerFlag = false;
                  window.location.href ="/erro404"
                }
              });
          }
          }
        }
        },
        error: (err) => {
          this.DidUserReserveTHisRoomFlag = false;
          window.location.href ="erro404"
        }
      });
    }


}
