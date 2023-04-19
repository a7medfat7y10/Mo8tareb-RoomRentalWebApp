import { Component, OnInit } from '@angular/core';
import { AccountApiService } from '../../account-api.service';
import { ActivatedRoute, Route, Router, RouterLink, Routes } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent   {

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

