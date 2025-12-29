import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AttendeeService } from '../attendee.service';

@Component({
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css']
})
export class EventDetailsComponent {
  event: any;

  constructor(
    route: ActivatedRoute,
    private service: AttendeeService,
    private router: Router
  ) {
    const id = Number(route.snapshot.paramMap.get('id'));
    this.event = this.service.getEventById(id);
  }

  buyTicket() {
    this.router.navigate(['/attendee/checkout', this.event.id]);
  }
}
