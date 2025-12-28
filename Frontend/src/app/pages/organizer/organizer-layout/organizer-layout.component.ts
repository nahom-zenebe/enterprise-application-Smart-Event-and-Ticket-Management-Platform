import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-organizer-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterOutlet],
  templateUrl: './organizer-layout.html',
  styleUrls: ['./organizer-layout.css']
})
export class OrganizerLayoutComponent {
  navItems = [
    { path: '/organizer/dashboard', label: 'Dashboard' },
    { path: '/organizer/events', label: 'My Events' },
    { path: '/organizer/event-editor', label: 'Create Event' },
    { path: '/organizer/orders', label: 'Attendees' },
    { path: '/organizer/settings', label: 'Settings' }
  ];
}

