import { Component, OnInit } from '@angular/core';
import { AccountApiService } from '../../Services/account-api.service';
import { ActivatedRoute, Route, Router, RouterLink, Routes } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent   {

  constructor(private translate: TranslateService){}
  isRtl(): boolean {
    const currentLang = this.translate.currentLang;
    return currentLang === 'ar';
  }

}

