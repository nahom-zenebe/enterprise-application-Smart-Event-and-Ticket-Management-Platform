import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-attendee',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './attendee.html',
  styleUrls: ['./attendee.css']
})
export class AttendeeComponent {
  featuredEvents = [
    { id: 1, name: 'Summer Music Festival', date: 'Jun 15, 2024', location: 'Central Park', price: '$50', image: '🎵' },
    { id: 2, name: 'Tech Conference: AI Future', date: 'Jul 22, 2024', location: 'Convention Center', price: '$120', image: '💻' },
    { id: 3, name: 'Art Exhibition Opening', date: 'Aug 5, 2024', location: 'Modern Art Gallery', price: '$25', image: '🎨' },
    { id: 4, name: 'Charity Gala Dinner', date: 'Sep 12, 2024', location: 'Grand Hotel Ballroom', price: '$150', image: '🍽️' }
  ];

  upcomingEvents = [
    { id: 5, name: 'Jazz Night', date: 'Oct 20, 2024', location: 'Jazz Club', price: '$35', image: '🎷' },
    { id: 6, name: 'Startup Pitch Night', date: 'Nov 5, 2024', location: 'Innovation Hub', price: '$40', image: '🚀' }
  ];
}

