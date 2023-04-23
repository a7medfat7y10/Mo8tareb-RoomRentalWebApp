import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent {
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
  ResetPasswordDto:any
  Email: string = "";
  clientURI : string= window.location.href
  APIError: string = '';


  sendSetPasswordDataToApi(email:any,newPassword:any,newPasswordConfirm:any) {

    if (this.validEmail() && this.APIError == '') {
      this.ResetPasswordDto = {
        "Email": email,
        "Password": newPassword,
        "ConfirmPassword": newPasswordConfirm,
        "Token":"",
        "clientURI": (this.clientURI).slice(0, (this.clientURI.length - "forgetPassword".length))
      }
      this.myCrud.CreateForgetPasswordCall(this.ResetPasswordDto).subscribe({
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
