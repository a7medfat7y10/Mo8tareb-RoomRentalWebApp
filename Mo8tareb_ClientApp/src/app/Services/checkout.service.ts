import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { CreateCheckoutSessionRequest } from '../Interfaces/CreateCheckoutSessionRequest';
import { CreateCheckoutSessionResponse } from '../Interfaces/CreateCheckoutSessionResponse';

// declare const Stripe;

@Injectable({
  providedIn: 'root',
})

export class CheckoutService {
  private readonly apiUrl = 'https://localhost:7188/api/Stripes/create-checkout-session';

  constructor(private readonly http: HttpClient) {}

  createCheckoutSession(
    request: CreateCheckoutSessionRequest
  ): Observable<CreateCheckoutSessionResponse> {
    return this.http.post<CreateCheckoutSessionResponse>(this.apiUrl, request);
  }
}


