import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountApiService } from './account-api.service';

@Injectable({
  providedIn: 'root'
})
export class UserControllerServiceService {
  private API_Uri = "https://localhost:7188/api/Users/";
  obj: any;
  constructor(private myClient: HttpClient,private Account: AccountApiService,) { }

  AssignUserToRoleOwner() {

    return this.myClient.get(this.API_Uri+"AssignRoleOwnerToUserAsync?Email="+this.Account.GetEmail())
  }
  getUserByEmail() {
    return this.myClient.get(this.API_Uri+"GetUserByEmail?Email="+this.Account.GetEmail())
  }
}
