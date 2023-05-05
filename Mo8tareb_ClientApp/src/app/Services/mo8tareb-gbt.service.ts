import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Mo8tarebGBTService {

  constructor(private client: HttpClient) { }
  SendQuery(msg: string) {
    return fetch(`https://localhost:7188/api/M08tarebGBT?query=${msg}`)
      .then(response => {
        console.log(response);  // add this line to check the response
        return response.json();
      });
  }

}
