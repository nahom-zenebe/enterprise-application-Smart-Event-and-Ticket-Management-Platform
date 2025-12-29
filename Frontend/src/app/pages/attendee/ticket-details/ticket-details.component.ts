import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  templateUrl: './ticket-details.component.html',
  styleUrls: ['./ticket-details.component.css']
})
export class TicketDetailsComponent {
  ticketId: string | null;

  constructor(route: ActivatedRoute) {
    this.ticketId = route.snapshot.paramMap.get('id');
  }
}
