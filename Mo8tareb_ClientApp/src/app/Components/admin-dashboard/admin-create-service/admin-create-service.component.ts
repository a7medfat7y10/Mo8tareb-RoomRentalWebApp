import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-admin-create-service',
  templateUrl: './admin-create-service.component.html',
  styleUrls: ['./admin-create-service.component.css']
})
export class AdminCreateServiceComponent {
  constructor(private readonly myClient: HttpClient,private route:Router) { }
  Add(name:any){
    let  newservice = {name};
    this.myClient.post("https://localhost:7188/api/Services/CreateService", newservice).subscribe();
    this.route.navigate(["/Admindashboard/services"]);
  }
}
