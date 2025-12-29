import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
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
