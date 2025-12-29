import { Component } from '@angular/core';
import { AttendeeService } from '../attendee.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-event-list',
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
