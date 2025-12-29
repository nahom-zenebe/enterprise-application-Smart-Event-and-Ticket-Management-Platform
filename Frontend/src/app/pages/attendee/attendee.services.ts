import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AttendeeService {

  events = [
    { id: 1, title: 'Music Festival', date: '2025-01-20', venue: 'Addis Arena', price: 500 },
    { id: 2, title: 'Tech Conference', date: '2025-02-05', venue: 'Millennium Hall', price: 800 }
  ];

  tickets = [
    { id: 101, event: 'Music Festival', date: '2025-01-20', status: 'Confirmed' }
  ];

  getEvents() {
    return this.events;
  }

  getEventById(id: number) {
    return this.events.find(e => e.id === id);
  }

  getMyTickets() {
    return this.tickets;
  }
}
