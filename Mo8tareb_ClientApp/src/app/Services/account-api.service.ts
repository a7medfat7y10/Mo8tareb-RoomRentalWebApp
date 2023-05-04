import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { json } from 'body-parser';

@Injectable({
  providedIn: 'root'
})
export class AccountApiService {


  private API_Uri = "https://localhost:7188/api/accounts/";

  constructor(private myClient: HttpClient) { }

  GetUserById(id: string) {
    return this.myClient.get("https://localhost:7188/api/users/"+`GetUserById?id=${id}` );
  }


  SignUP(user:any)
  {
    return this.myClient.post(this.API_Uri+"Registration", user);
  }
  ConfirmEmailRegistration(QueryString:String) {
    return this.myClient.get(this.API_Uri+"EmailConfirmation?"+QueryString);
  }
  SignIn(user:any) {
    return this.myClient.post(this.API_Uri+"Login",user)
  }
  CreateForgetPasswordCall(user:any)
  {
    return this.myClient.post(this.API_Uri+"ForgotPassword", user);
  }
  ResetPassword(user:any) {
    return this .myClient.post(this.API_Uri+"ResetPassword",user)
  }
  StoreEmail(Email:any) {
    localStorage.setItem("UserEmail", Email);
  }
  GetEmail() {
   return localStorage.getItem("UserEmail");
  }
  StoreToken(tokenValue:string) {
    localStorage.setItem("M08tarebToken",tokenValue);
  }
  GetToken() {
    return localStorage.getItem("M08tarebToken")
  }
  StoreRole(Role:any) {
    localStorage.setItem("RoleOfUser",Role);
  }
  SignOut() {
    localStorage.clear();
    window.location.href = "/login";
  }
  GetRole() {
    return localStorage.getItem("RoleOfUser")
  }
  IsLoggedIn():boolean {
    return  !!this.GetToken()
  }



}
