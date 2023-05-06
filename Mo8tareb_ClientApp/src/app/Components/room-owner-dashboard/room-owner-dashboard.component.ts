import { Component, OnInit } from '@angular/core';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';

@Component({
  selector: 'app-room-owner-dashboard',
  templateUrl: './room-owner-dashboard.component.html',
  styleUrls: ['./room-owner-dashboard.component.css']
})
export class RoomOwnerDashboardComponent implements OnInit {
  constructor(private accountServise: AccountApiService){}
  myEmail: any;

  ngOnInit(): void {
    this.myEmail = this.accountServise.GetEmail();
  }

}
