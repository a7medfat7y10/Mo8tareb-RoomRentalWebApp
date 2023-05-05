import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-services',
  templateUrl: './admin-services.component.html',
  styleUrls: ['./admin-services.component.css']
})
export class AdminServicesComponent implements OnInit{
  services:any;
  serviceToDelete: any = {}; // create an empty object of ServicesToDeleteDtos class
  constructor(public myClient:HttpClient, public route:Router){

  }
  ngOnInit(): void {

    this.myClient.get("https://localhost:7188/api/Services/GetAllServies").subscribe({
      next:(data:any)=>{this.services = data
      console.log(data);
      },
      error:()=>{}
    });
  }
  Delete(id:number){
    this.serviceToDelete.id = id; // set the id in the ServicesToDeleteDtos object
    this.myClient.delete("https://localhost:7188/api/Services/DeleteService?id="+ id, { body: this.serviceToDelete }).subscribe({
      next: () => {
        console.log('Service deleted successfully');
        this.route.navigateByUrl('/', { skipLocationChange: true }).then(() => {
          this.route.navigate(['/Admindashboard/services']);
        });
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
