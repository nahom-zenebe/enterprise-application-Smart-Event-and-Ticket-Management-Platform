import { Component } from '@angular/core';
import { AttendeeService } from '../attendee.service';
import { Router } from '@angular/router';

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
