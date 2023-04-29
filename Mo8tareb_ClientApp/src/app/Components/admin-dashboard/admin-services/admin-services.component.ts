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
    this.myClient.delete("https://localhost:7188/api/Services/DeleteService?id"+ id).subscribe();
    console.log(id);
    this.route.navigate(["/Admindashboard/services"]);
  }
}
