import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-revenue-attendee-chart',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './revenue-attendee-chart.html',
  styleUrls: ['./revenue-attendee-chart.css']
})
export class RevenueAttendeeChartComponent {
  selectedPeriod = 'Last 7 days';
}

