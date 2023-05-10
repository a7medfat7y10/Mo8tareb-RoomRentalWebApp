import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Route, Router } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';

import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  showNavbar:any;
  isCollapsed:boolean = false;
  constructor(private ActivatedRoute:ActivatedRoute,private AuthenticationService: AccountApiService,public translate: TranslateService) {
    // this.translate.setDefaultLang('en');
    const storedLang = localStorage.getItem('lang');
    if (storedLang) {
      this.translate.use(storedLang);
    } else {
      this.translate.setDefaultLang('en');
    }

   }


  SignOut() {
    this.AuthenticationService.SignOut();
    window.location.href = "/login";
  }

  UserIsLoggedIn():boolean {

    if (this.AuthenticationService.GetToken())
    return true;

    return false;
  }


  // useLanguage(event: Event): void {
  //   const language = (event.target as HTMLSelectElement).value;
  //   this.translate.use(language);
  // }

  useLanguage(event: Event): void {
    const language = (event.target as HTMLSelectElement).value;
    this.translate.use(language);
    localStorage.setItem('lang', language);
  }

  toggleMenu(){
    if(this.isCollapsed == true)
      this.isCollapsed = false
    else
    this.isCollapsed = true
  }

}
