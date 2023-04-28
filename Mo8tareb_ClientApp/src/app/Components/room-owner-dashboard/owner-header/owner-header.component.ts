import { Component } from '@angular/core';
import { AccountApiService } from 'src/app/Services/account-api.service';

@Component({
  selector: 'app-owner-header',
  templateUrl: './owner-header.component.html',
  styleUrls: ['./owner-header.component.css']
})
export class OwnerHeaderComponent {
  constructor(private AuthenticationService: AccountApiService){

  }
  SignOut() {
    this.AuthenticationService.SignOut();
    window.location.href = "/login";
  }
}
