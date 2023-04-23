import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AccountApiService } from '../../Services/account-api.service';
import {  ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-rest-password',
  templateUrl: './rest-password.component.html',
  styleUrls: ['./rest-password.component.css']
})
export class RestPasswordComponent implements OnInit{

  @ViewChild('email') EmailInput: ElementRef<HTMLInputElement> | undefined;

  token: any = '';
  email: any = '';

  constructor(private myCrud:AccountApiService,private route:Router,private myActivated:ActivatedRoute) {
  }
  ngOnInit(): void {

    this.token = this.myActivated.snapshot.queryParams["token"];
    this.email = this.myActivated.snapshot.queryParams["email"];

    if (!this.token || !this.email)
      this.route.navigate(["/unAuthorized/"]);

    if(this.EmailInput){
      this.EmailInput.nativeElement.value = this.email;
      this.EmailInput.nativeElement.disabled = true;
    }
  }
  formValidation = new FormGroup({
    email: new FormControl("", [Validators.email, Validators.required]),
    password: new FormControl("", [ Validators.required]),
    confirmPassword: new FormControl("", [Validators.required])
  });

  validEmail() {
    return this.formValidation.controls["email"].valid
  }
  validPassword() {
    return this.formValidation.controls["password"].valid
  }
  validconfirmPassword() {
    return this.formValidation.controls["confirmPassword"].valid
  }
  IsValidData() {
    return this.validEmail() && this.validPassword() && this.validconfirmPassword();
  }
  resetApiError() {
    this.APIError = '';
  }
  getAPIError() {
    return this.APIError;
  }
  Email: string = "";
  clientURI : string= window.location.href
  APIError: string = '';


  user: any = {
    "Password":String ,
    "ConfirmPassword": String,
    "Email": String,
    "Token":String
  }
  sendForgetPasswordToApi(email:any,password:any,confirmPassword:any) {

    if (this.IsValidData() && this.APIError == '' && password==confirmPassword) {

      this.user = {
        "Email": email,
        "Password":password ,
        "ConfirmPassword":confirmPassword ,
        "Token": this.token
      }


      this.myCrud.ResetPassword(this.user).subscribe({
        next: (data: any) => {

          this.APIError == '';
          this.myCrud.SignOut();
        },
        error: (err: any) => {
          this.APIError = err;

        }
      })
    }
    else{
      alert("can not submit , please Enter Valid Data !");
      window.location.reload();
    }
  }
}
