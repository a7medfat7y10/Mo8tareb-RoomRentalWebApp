import { NotExpr } from '@angular/compiler';
import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { urlencoded } from 'body-parser';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent
{
  constructor(private myService:AccountApiService ,private router:Router,private translate: TranslateService) {
    const storedLang = localStorage.getItem('lang');
    if (storedLang) {
      this.translate.use(storedLang);
    } else {
      this.translate.setDefaultLang('en');
    }
  }

  // Custom validator function to compare password and confirm password fields
  passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const passwordValidation = control.get('Password');
    const confirmPassword = control.get('PasswordConfirm');
    if (passwordValidation?.value !== confirmPassword?.value) {
      control.get('PasswordConfirm')?.setErrors({ 'passwordMismatch': true });
      return { 'passwordMismatch': true };
    } else {
      control.get('PasswordConfirm')?.setErrors(null);
      return null;
    }
  }



    formValidation = new FormGroup({
    fname: new FormControl("", [Validators.maxLength(30), Validators.minLength(1), Validators.required,Validators.pattern(/^[a-zA-Z]+$/)]),
    lname: new FormControl("", [Validators.maxLength(30), Validators.minLength(1), Validators.required,Validators.pattern(/^[a-zA-Z]+$/)]),
    userName: new FormControl("", [Validators.maxLength(30), Validators.minLength(1), Validators.required]),
    email: new FormControl("", [Validators.email, Validators.required]),
      phone: new FormControl("", [Validators.pattern(/^0(11|12|15|10|16|18)?\d{8}$/), Validators.required]),
      Password: new FormControl("", [Validators.required]),
    PasswordConfirm: new FormControl("", [Validators.required, this.passwordMatchValidator.bind(this)])
  });


  validFname() {
    return this.formValidation.controls["fname"].valid
  }

  validUserName() {
    return this.formValidation.controls["userName"].valid
  }
  validLname() {
    return this.formValidation.controls["lname"].valid
  }
  validEmail() {
    return this.formValidation.controls["email"].valid
  }
  validPhone() {
    return this.formValidation.controls["phone"].valid
  }
  validPassword() {
    return this.formValidation.controls["Password"].valid
  }
  validPasswordConfirm() {
    return this.formValidation.controls["PasswordConfirm"].valid
  }
  getAPIError() {
    return this.ApiErrorVariable;
  }
  getFormErrors() {
    if (this.validFname() &&this.validPhone() && this.validLname() && this.validEmail() && this.validPassword() && this.validPasswordConfirm())
    return true;

    return false
  }

  resetApiError() {
    this.ApiErrorVariable = '';
  }

  NavigateEmailConfirmation() {
    // this.router.navigate(["/EmailConfirmation/"])
    window.location.href = "/EmailConfirmation/";
  }

  ApiErrorVariable: string = "";

  user: any = {
    "firstName": "string",
    "lastName": "string",
    "userName": "string",
    "gender": "",
    "email": "string",
    "phone": "string",
    "password": "string",
    "confirmPassword": "string",
    "clientURI": window.location.href
  };

  FirstName: string = "";
  LastName: string = "";
  Gender: string = "";
  phone : any = "";
  userName: string = "";
  Email: string = "";
  Password: string = "";
  PasswordConfirm: string = "";
  URL: string = window.location.href;



  CreateUser(fname:string,lname:string,userName:string,email:string,phoneNumber:any,Gender:string,pass:string,rePass:string)
  {

    if (this.getFormErrors() && pass == rePass &&  this.ApiErrorVariable == '' )
    {
      this.FirstName = fname;
      this.LastName = lname;
      this.userName = userName;
      this.phone = phoneNumber;
      this.Gender = Gender;
      this.Email = email;
      this.Password = pass;
      this.PasswordConfirm = rePass;
    //errors

    this.user ={
    "FirstName": this.FirstName,
    "LastName": this.LastName,
    "UserName": this.userName,
    "Gender": this.Gender,
    "Email": this.Email,
    "phone": this.phone,
    "Password": this.Password,
    "ConfirmPassword": this.PasswordConfirm,
      "ClientURI":this.URL.slice(0,(this.URL.length-"register".length))+"loading"
    }
console.log(this.user)
    this.myService.SignUP(this.user).subscribe({
      next: (data: any) => {
        this.ApiErrorVariable == '';
        console.log(data)
        
        this.NavigateEmailConfirmation();
        },
      error: (err: any) => {
        this.ApiErrorVariable = err.message;
      console.log(err)}
      });
    }
    else{
      alert("can not submit , please Enter Valid Data !");
      window.location.reload();
    }
  }
}
