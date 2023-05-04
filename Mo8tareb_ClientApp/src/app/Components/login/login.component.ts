import { Token } from '@angular/compiler';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountApiService } from 'src/app/Services/account-api.service';

interface LoginResponse {
  isAuthSuccessful: boolean;
  errorMessage: string;
  token: string;
}
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(private myCrud:AccountApiService) {
  }

  ApiError: string = '';
  // Email: string = "";
  clientURI : string= window.location.href
  ApiErrorVariable: string = '';
  // Password: string = '';

  formValidation = new FormGroup({
    email: new FormControl("", [Validators.email, Validators.required]),
    Password: new FormControl("", [Validators.required])
  });

  LogInDtos: any = {};
  validEmail() {
    return this.formValidation.controls["email"].valid
  }
  validPassword() {
    return this.formValidation.controls["Password"].valid
  }

   LoginResponse:any= {
    "isAuthSuccessful": Boolean,
    "errorMessage": String,
    "token": String
}
  resetApiError() {
    this.ApiErrorVariable = '';
}
  LogIn(email: any, password: any) {

    if (this.validEmail() && this.validPassword() && this.ApiErrorVariable == '') {

      this.LogInDtos = {
        "Email": email,
        "Password":password,
        "clientURI": this.clientURI
      }
      console.log(this.LogInDtos.Email)
      console.log(this.LogInDtos.Password)
      console.log(this.LogInDtos.clientURI)
      this.myCrud.SignIn(this.LogInDtos).subscribe({
        next: (res:any) => {
          this.ApiErrorVariable == '';

          //token
          this.myCrud.StoreToken(res.token);
          //Roles
          this.myCrud.StoreRole(res.role);
          //Email
          this.myCrud.StoreEmail(res.email);
          if(this.myCrud.GetRole()?.includes("Admin"))
            window.location.href = "/Admindashboard/";
          else if(this.myCrud.GetRole()?.includes("Owner"))
            window.location.href = "/OwnerDashboard/";
          else
           window.location.href = "/home/";
        },
        error: (err:any) => { this.ApiErrorVariable = err }
      })
    }
    else{
      alert("can not submit , please Enter Valid Data !");
      window.location.reload();
    }

  }
  getAPIError() {
    return this.ApiErrorVariable;
  }
}
