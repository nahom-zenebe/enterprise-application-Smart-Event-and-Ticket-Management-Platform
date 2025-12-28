import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MetricsCardsComponent } from './components/metrics-cards/metrics-cards.component';
import { RevenueAttendeeChartComponent } from './components/revenue-attendee-chart/revenue-attendee-chart.component';
import { OrganizerBarChartComponent } from './components/organizer-bar-chart/organizer-bar-chart.component';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, MetricsCardsComponent, RevenueAttendeeChartComponent, OrganizerBarChartComponent,],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class AdminDashboardComponent {}

