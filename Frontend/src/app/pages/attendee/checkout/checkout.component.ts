import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent {
  quantity = 1;

  constructor(private router: Router) {}

  confirm() {
    alert('Payment successful (mock)');
    this.router.navigate(['/attendee/my-tickets']);
  }
}
