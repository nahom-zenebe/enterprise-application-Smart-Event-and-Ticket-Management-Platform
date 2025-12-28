import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-tickets-pricing-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tickets-pricing-form.html',
  styleUrls: ['./tickets-pricing-form.css']
})
export class TicketsPricingFormComponent {
  ticketTypes = [
    { name: 'General Admission', price: 0, quantity: 0 }
  ];

  addTicketType() {
    this.ticketTypes.push({ name: '', price: 0, quantity: 0 });
  }

  removeTicketType(index: number) {
    if (this.ticketTypes.length > 1) {
      this.ticketTypes.splice(index, 1);
    }
  }
}

