import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AttendeeService } from '../attendee.services';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {

  events: any[] = [];

  constructor(
    private service: AttendeeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.events = this.service.getEvents();
  }

  openEvent(id: number): void {
    this.router.navigate(['/attendee/events', id]);
  }
}
