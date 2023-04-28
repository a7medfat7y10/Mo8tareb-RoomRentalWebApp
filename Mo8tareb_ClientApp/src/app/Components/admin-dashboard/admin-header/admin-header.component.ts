import { Component } from '@angular/core';
import { AccountApiService } from 'src/app/Services/account-api.service';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class AdminHeaderComponent {
  constructor(private AuthenticationService: AccountApiService){

  }
  SignOut() {
    this.AuthenticationService.SignOut();
    window.location.href = "/login";
  }
}
