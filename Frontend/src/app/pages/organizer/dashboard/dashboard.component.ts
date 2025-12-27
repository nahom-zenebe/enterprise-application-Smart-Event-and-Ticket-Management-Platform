import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { KpiCardsComponent } from './components/kpi-cards/kpi-cards.component';
import { RevenueChartComponent } from './components/revenue-chart/revenue-chart.component';
import { RecentEventsComponent } from './components/recent-events/recent-events.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, KpiCardsComponent, RevenueChartComponent, RecentEventsComponent],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent {}

