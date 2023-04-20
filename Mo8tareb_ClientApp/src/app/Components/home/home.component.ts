import { Component, OnInit } from '@angular/core';
import { AccountApiService } from '../../account-api.service';
import { ActivatedRoute, Route, Router, RouterLink, Routes } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent   {


  rooms: { id: number, location: string, price: number, roomType:string , ownerId: string, owner: {},
  reservations: {}[], reviews: {}[], services:{}[], images:{id:number, imageUrl:string}[]}[] = [];





  constructor(private http: HttpClient, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.getRooms();
  }

  getRooms() {
    const apiUrl = 'https://localhost:7188/GetAllRooms';
    this.http.get<any>(apiUrl).subscribe(
      (data) => {
        data.forEach((room:any) => {
          this.rooms.push(room);
        });
        this.rooms.forEach((room:any) => {
          room.images = room.images.map((image:any) =>({
            id: image.id,
            imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
          }))
        })
      },
      error => {
        console.log(error);
      }
    );

  }

}

