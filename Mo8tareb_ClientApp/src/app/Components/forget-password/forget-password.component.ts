import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountApiService } from '../../Services/account-api.service';
import { __extends, __decorate } from "tslib/tslib";
import { Router } from '@angular/router';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css']
})
export class ForgetPasswordComponent {


  constructor(private myCrud:AccountApiService,private route:Router) {
  }
  formValidation = new FormGroup({
    email: new FormControl("", [Validators.email, Validators.required])
  });

  validEmail() {
    return this.formValidation.controls["email"].valid
  }
  resetApiError() {
    this.APIError = '';
  }
  getAPIError() {
    return this.APIError;
  }
  ForgotPasswordDto:any
  Email: string = "";
  clientURI : string= window.location.href
  APIError: string = '';


  sendForgetPasswordToApi(email:any) {

    if (this.validEmail() && this.APIError == '') {
      this.ForgotPasswordDto = {
        "Email": email,
        "clientURI": ((this.clientURI).slice(0, (this.clientURI.length - "forgetPassword".length))) + "resetPassword"
      }
      this.myCrud.CreateForgetPasswordCall(this.ForgotPasswordDto).subscribe({
        next: (data: any) => {
          this.APIError == '';
      this.route.navigate(["/EmailConfirmation/"]);
        },
        error: (err: any) => {
          console.log(err)
          this.APIError = err
        }
      });
    }
    else {
      alert("can not submit , please Enter Valid Data !");
      window.location.reload();
    }

  }

}
