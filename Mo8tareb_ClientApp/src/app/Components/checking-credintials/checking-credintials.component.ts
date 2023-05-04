import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';

@Component({
  selector: 'app-checking-credintials',
  templateUrl: './checking-credintials.component.html',
  styleUrls: ['./checking-credintials.component.css']
})
export class CheckingCredintialsComponent implements OnInit {
  constructor(private ActivatedRoute:ActivatedRoute,private AuthenticationService: AccountApiService) { }

  token: String = '';
  email: String = '';

  ngOnInit(): void {
    this.token= this.ActivatedRoute.snapshot.queryParams["token"];
    this.email = this.ActivatedRoute.snapshot.queryParams["email"];
console.log(this.token)
console.log(this.email)
    this.AuthenticationService.ConfirmEmailRegistration(`email=${this.email}&token=${this.token}`).subscribe({
      next: (response: any) => {
        if (response.isAuthSuccessful == true) {
          //token
          this.AuthenticationService.StoreToken(response.token);
          //Roles
          this.AuthenticationService.StoreRole(response.role);
          //Email
          this.AuthenticationService.StoreEmail(this.email);
          if (this.AuthenticationService.GetRole()?.includes("Admin"))
            window.location.href = "/AdminDashboard/";
          else if (this.AuthenticationService.GetRole()?.includes("Owner"))
            window.location.href = "/RoomOwnerDashboard/";
          else
            window.location.href = "/home/";
        }
        else
          window.location.href = "/Error404/";
      },
      error: (err: any) => {
        console.log(err);
        window.location.href = "/Error404/";
}
    });
  }

}
