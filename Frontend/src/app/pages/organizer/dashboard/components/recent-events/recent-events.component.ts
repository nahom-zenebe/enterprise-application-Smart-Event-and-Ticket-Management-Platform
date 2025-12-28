import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-recent-events',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './recent-events.html',
  styleUrls: ['./recent-events.css']
})
export class RecentEventsComponent {}

