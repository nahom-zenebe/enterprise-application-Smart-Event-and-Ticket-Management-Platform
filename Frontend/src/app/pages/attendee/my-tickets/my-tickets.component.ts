import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AttendeeService } from '../attendee.service';

@Component({
  templateUrl: './my-tickets.component.html',
  styleUrls: ['./my-tickets.component.css']
})
export class MyTicketsComponent {
  tickets = this.service.getMyTickets();

  constructor(private service: AttendeeService, private router: Router) {}

  viewTicket(id: number) {
    this.router.navigate(['/attendee/ticket', id]);
  }
}
