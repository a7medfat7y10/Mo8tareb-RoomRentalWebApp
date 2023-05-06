import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-payment-failure',
  templateUrl: './payment-failure.component.html',
  styleUrls: ['./payment-failure.component.css']
})
export class PaymentFailureComponent implements OnInit {
    roomId:any;
    constructor(private route: ActivatedRoute) { }

    ngOnInit(): void {
      this.route.params.subscribe(params => {
      this.roomId = params['id'];
    });
    }

}
