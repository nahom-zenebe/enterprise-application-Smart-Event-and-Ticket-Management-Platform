import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AttendeeService } from '../attendee.services';

@Component({
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent {
  events = this.service.getEvents();
  constructor(private service: AttendeeService, private router: Router) {}
  openEvent(id: number) {
    this.router.navigate(['/attendee/events', id]);
  }
}
