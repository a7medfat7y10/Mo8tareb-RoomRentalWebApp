import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
//import { environment } from 'src/environments/environment';
//import { ICustomerPortal, IMemberShipPlan, ISession } from './IMembership';
import { CreateCheckoutSessionRequest } from '../Interfaces/CreateCheckoutSessionRequest';
import { CreateCheckoutSessionResponse } from '../Interfaces/CreateCheckoutSessionResponse';

// declare const Stripe;

@Injectable({
  providedIn: 'root',
})
//

export class CheckoutService {
  private readonly apiUrl = 'https://localhost:7188/api/Stripes/create-checkout-session';

  constructor(private readonly http: HttpClient) {}

  createCheckoutSession(
    request: CreateCheckoutSessionRequest
  ): Observable<CreateCheckoutSessionResponse> {
    return this.http.post<CreateCheckoutSessionResponse>(this.apiUrl, request);
  }
}



//  export class MembershipService {
  //   baseUrl: string = environment.baseUrl;

  //   constructor(private http: HttpClient) {}

  //   getMembership(): Observable<IMemberShipPlan> {
  //     return of({
  //       id: '',
  //       priceId: 'Dont forget to add your price id ',
  //       name: 'Awesome Membership Plan',
  //       price: '$9.00',
  //       features: [
  //         'Up to 5 users',
  //         'Basic support on Github',
  //         'Monthly updates',
  //         'Free cancelation',
  //       ],
  //     });
  //   }
  //   requestMemberSession(priceId: string): void {
  //     this.http
  //       .post<ISession>(this.baseUrl + 'api/payments/create-checkout-session', {
  //         priceId: priceId,
  //         successUrl: environment.successUrl,
  //         failureUrl: environment.cancelUrl,
  //       })
  //       .subscribe((session) => {
  //         this.redirectToCheckout(session);
  //       });
  //   }

  //   redirectToCheckout(session: ISession) {
  //     const stripe = Stripe(session.publicKey);

  //     stripe.redirectToCheckout({
  //       sessionId: session.sessionId,
  //     });
  //   }

  //   getHttpOptions() {
  //     const httpOptions = {
  //       headers: new HttpHeaders({
  //         Authorization: 'Bearer ' + localStorage.getItem('token'),
  //       }),
  //     };

  //     return httpOptions;
  //   }
  // }
//}
