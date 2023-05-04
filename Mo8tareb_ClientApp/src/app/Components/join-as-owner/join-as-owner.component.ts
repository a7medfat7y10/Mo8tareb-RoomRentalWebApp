import { Component } from '@angular/core';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { UserControllerServiceService } from 'src/app/Services/user-controller-service.service';

@Component({
  selector: 'app-join-as-owner',
  templateUrl: './join-as-owner.component.html',
  styleUrls: ['./join-as-owner.component.css']
})
export class JoinAsOwnerComponent {

  constructor(private service: UserControllerServiceService,private Account: AccountApiService) {


  }

  AssignRole(){
    this.service.AssignUserToRoleOwner().subscribe({
      next: (data) => {
        this.Account.SignOut();
        window.location.href="/login"
      },
      error: (err) => {
        window.location.href="/error404"

      }
    });
  }



}
