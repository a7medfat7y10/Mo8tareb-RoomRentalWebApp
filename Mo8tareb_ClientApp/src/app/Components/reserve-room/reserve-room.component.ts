import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { ReservationServiceService } from 'src/app/Services/reservation-service.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-reserve-room',
  templateUrl: './reserve-room.component.html',
  styleUrls: ['./reserve-room.component.css']
})
export class ReserveRoomComponent implements OnInit {

  constructor(private ReservationService:ReservationServiceService,myActivate: ActivatedRoute, private roomCrud: RoomServiceService, private sanitizer: DomSanitizer,
  private AccountService:AccountApiService
  ) {
    this.roomId = myActivate.snapshot.params["id"];
  }
  formValidation = new FormGroup({
    email: new FormControl("", [Validators.email, Validators.required]),
    Arrival: new FormControl("", [ Validators.required]),
    departure: new FormControl("", [ Validators.required])
  });



  validEmail() {
    return this.formValidation.controls["email"].valid;
  }
  validArrivalDate() {
    return this.formValidation.controls["Arrival"].valid;
  }
  validDepartureDate() {
    return this.formValidation.controls["departure"].valid;
  }
  getAPIError() {
    return this.APIVariableError;
  }
  roomId = 0;
  room: any;
  RoomImage: any;
  owner: any= {
    FirstName: '',
    LastName: ''
  };;
  FirstName: string = '';
  LastName: string = '';
  APIVariableError: any = '';
  reservation: any;


  ngOnInit(): void {

    this.formValidation.controls['email'].setValue( this.AccountService.GetEmail());

     this.roomCrud.getRoomById(this.roomId).subscribe({
       next: (data:any) => {
         this.room = data;
         this.room.images = this.room.images.map((image: any) => ({
             id: image.id,
             imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
         }));
         this.AccountService.GetUserById(this.room.ownerId).subscribe({
           next: (data: any) => {

             this.FirstName = data.firstName;
             this.LastName = data.lastName;

          },
          error: (err) =>{
            console.log(err);
          }
        });
        }
      ,
      error: (err) => {
    console.log(err);}
     });
  }
  resetApiError() {
    this.APIVariableError = '';
  }
  getReservationForm(ArrivalDate: string, departureDate: string, Email: string)
  {
    if (this.validEmail() && this.validArrivalDate() && this.validDepartureDate())
    {
      if (new Date(ArrivalDate) > new Date(departureDate))
      {
        alert("Arrival Date Cannot be larger than Departure Date !");
        window.location.reload();
        return;
      }
      //call for the API here
      this.reservation = {
        "startDate": ArrivalDate ,
        "endDate": departureDate ,
        "status": 0,
        "user": {
          "email": Email
        },
        "room": {
          "id": +this.roomId
        }
      }
      console.log(this.reservation);

      //Bug ?????????????????????????????????????????????????????????????????????????????????????????????????????
      this.ReservationService.CreateReservation(this.reservation).subscribe({
        next: (data) => {
      console.log(data);
          this.APIVariableError = '';
          window.location.href = `ReservationSuspended/${this.roomId}`
        },
        error: (err) => {
          this.APIVariableError = '';
          window.location.href = `ReservationSuspended/${this.roomId}`
          // console.log(err);
        }
      });
    }
    else
    {
        alert("can not submit , please Enter Valid Data !");
        window.location.reload();
    }
  }


}
