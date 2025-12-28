import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './users.html',
  styleUrls: ['./users.css']
})
export class AdminUsersComponent {
  users = [
    { id: 'USR-001', name: 'John Doe', email: 'john@example.com', role: 'Organizer', status: 'Active', events: 12, joined: 'Jan 2024' },
    { id: 'USR-002', name: 'Jane Smith', email: 'jane@example.com', role: 'Attendee', status: 'Active', events: 0, joined: 'Feb 2024' },
    { id: 'USR-003', name: 'Bob Johnson', email: 'bob@example.com', role: 'Organizer', status: 'Active', events: 8, joined: 'Mar 2024' },
    { id: 'USR-004', name: 'Alice Brown', email: 'alice@example.com', role: 'Attendee', status: 'Inactive', events: 0, joined: 'Apr 2024' },
    { id: 'USR-005', name: 'Charlie Wilson', email: 'charlie@example.com', role: 'Organizer', status: 'Active', events: 15, joined: 'May 2024' }
  ];
}

