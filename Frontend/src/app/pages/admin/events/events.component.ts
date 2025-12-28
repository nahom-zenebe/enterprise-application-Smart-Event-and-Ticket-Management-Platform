import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-events',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './events.html',
  styleUrls: ['./events.css']
})
export class AdminEventsComponent {
  events = [
    { id: 'EVT-001', name: 'Summer Music Festival', organizer: 'John Doe', date: 'Jun 15, 2024', status: 'Active', tickets: 845, revenue: 42250 },
    { id: 'EVT-002', name: 'Tech Conference', organizer: 'Jane Smith', date: 'Jul 22, 2024', status: 'Active', tickets: 320, revenue: 24000 },
    { id: 'EVT-003', name: 'Art Exhibition', organizer: 'Bob Johnson', date: 'Aug 5, 2024', status: 'Draft', tickets: 0, revenue: 0 },
    { id: 'EVT-004', name: 'Charity Gala', organizer: 'Alice Brown', date: 'Sep 12, 2024', status: 'Upcoming', tickets: 90, revenue: 9000 }
  ];
}

