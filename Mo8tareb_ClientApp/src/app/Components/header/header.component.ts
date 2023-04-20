import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountApiService } from 'src/app/account-api.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  constructor(private ActivatedRoute:ActivatedRoute,private AuthenticationService: AccountApiService) { }

  SignOut() {
    this.AuthenticationService.SignOut();
    window.location.href = "/login";
  }

  UserIsLoggedIn():boolean {

    if (this.AuthenticationService.GetToken())
    return true;

    return false;
  }



}
