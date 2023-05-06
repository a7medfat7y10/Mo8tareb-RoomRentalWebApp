import { Component, OnInit } from '@angular/core';
import { AccountApiService } from 'src/app/Services/account-api.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent  implements OnInit {
  constructor(private accountServise: AccountApiService){}
  myEmail: any;

  ngOnInit(): void {
    this.myEmail = this.accountServise.GetEmail();
  }

}
