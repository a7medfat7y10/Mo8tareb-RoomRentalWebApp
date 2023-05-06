import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
//import { Stripe } from '@stripe/stripe-js';
//import { Stripe } from '@stripe/stripe-js/dist/Stripe';
// import { HttpClient } from '@angular/common/http';
import { CheckoutService } from '../../Services/checkout.service';
import { CreateCheckoutSessionRequest } from '../../Interfaces/CreateCheckoutSessionRequest';
import { CreateCheckoutSessionResponse } from '../../Interfaces/CreateCheckoutSessionResponse';
import { RoomServiceService } from 'src/app/Services/room-service.service';

declare const Stripe: any;
@Component({
  selector: 'app-reservation-approved',
  templateUrl: './reservation-approved.component.html',
  styleUrls: ['./reservation-approved.component.css'],
})
export class ReservationApprovedComponent implements OnInit {
  roomId: any;
  room: any;
  sessionId?: string;
  stripe?: any;

  constructor(
    readonly checkoutService: CheckoutService,
    private route: ActivatedRoute,
    private roomService: RoomServiceService
  ) {}

  ngOnInit(): void {
    //this.stripe = Stripe('pk_test_51N1EzlG1jmGhS2Ff51h9ePxxyj5RnmkHczPYHPc38DGJUFKKeRTQ9OKS93CTKrUlwKrEZtFnXKJj1GZKj0T0QmFS00aj7PxqwV');
    this.roomId = this.route.snapshot.params['id'];
    console.log('room id', this.roomId);
    this.roomService.getRoomById(this.roomId).subscribe({
      next: (data) => {
        this.room = data;
        console.log(this.room);
      },
      error: (error) => {
        console.log('HIIIIIIHHHH', error);
      },
    });
  }

  Pay() {
    const checkoutRequest: CreateCheckoutSessionRequest = {
      roomPrice: this.room.price, // set the room price here
      reservationId: this.room.reservations.id,
      roomId: this.roomId,
      roomDescription: this.room.description,
      //roomImages: this.room.images,
      successUrl: 'http://localhost:4200/payment-success',
      failureUrl: 'http://localhost:4200/payment-failure/' + this.roomId,
    };
    console.log(checkoutRequest);

    this.checkoutService.createCheckoutSession(checkoutRequest).subscribe({
      next: (response: CreateCheckoutSessionResponse) => {
        this.sessionId = response.sessionId;
        const stripe = Stripe(response.publicKey);
        stripe.redirectToCheckout({ sessionId: response.sessionId });
      },
      error: (error: any) => {
        console.log('HIIIIii');
        console.log(error);
      },
    });
  }
}
