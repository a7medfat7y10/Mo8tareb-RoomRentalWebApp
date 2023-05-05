import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-owner-create-room',
  templateUrl: './owner-create-room.component.html',
  styleUrls: ['./owner-create-room.component.css']
})
export class OwnerCreateRoomComponent implements OnInit {

  constructor(public myService: RoomServiceService, public myClient: HttpClient, private AccountService:AccountApiService) { }

  roomId!: number;
  selectedFile!: File;

  services!: any[];
  selectedServices: any[] = [];
  location: string = '';
  price: number = 700;
  roomType: string = '';
  description: string = '';
  numOfBeds: number = 1;
  ownerId:any;

  roomCreated:boolean = false;
  imageAdded:boolean = false;

  ngOnInit(): void {
    console.log(this.AccountService.GetEmail())

    this.myClient.get("https://localhost:7188/api/Services/GetAllServies").subscribe({
      next: (data: any) => {
        this.services = data;
      },
      error: () => { }
    });

    this.myClient.get("https://localhost:7188/api/Users/GetUserByEmail?Email=" + this.AccountService.GetEmail()).subscribe({
      next:(data:any)=>{
        this.ownerId = data.id;
      },
      error:()=>{}
    });

  }

  onServiceChange(event: any) {
    if (event.target.checked) {
      this.selectedServices.push(event.target.value);
    } else {
      const index = this.selectedServices.indexOf(event.target.value);
      if (index > -1) {
        this.selectedServices.splice(index, 1);
      }
    }
  }



  Add(location: string, price: number, roomType: string, description: string, numOfBeds: number, services: any[]) {
    let myServices: { id: number, name: string }[] = [];

    this.services.forEach(service => {
      if (services.indexOf(service.id.toString()) != -1) {
        myServices.push({ id: service.id, name: service.name });
      }
    });

    // console.log(myServices);


    let newroom = { location, price, roomType,description, numOfBeds , ownerId: this.ownerId, isreserved:false ,services: myServices };
    console.log(newroom);




    this.myService.AddNewRoom(newroom).subscribe((data: any) => {
      console.log(data);
      this.roomId = data;
      this.roomCreated = true;
    });
  }





  onSubmit() {
    console.log(this.roomId);
    const formData = new FormData();
    formData.append('RoomId', this.roomId.toString());
    formData.append('ImageUrl', this.selectedFile, this.selectedFile.name);
    this.myClient.post('https://localhost:7188/api/Images', formData).subscribe(
      (response) => {
        console.log('Data saved successfully');
        window.alert('Data saved successfully');
        if(!this.imageAdded){
          this.imageAdded = true;
        }
      },
      (error: HttpErrorResponse) => {
        // console.log('Error saving data');
        // console.log(error);
        // window.alert('Error saving data');

        console.log(`Error saving data. Status code: ${error.status}. Message: ${error.message}`);
        alert(`Error saving data. Status code: ${error.status}. Message: ${error.message}`);

      }
    );
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }


}
