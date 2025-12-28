import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-organizer-bar-chart',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './organizer-bar-chart.html',
  styleUrls: ['./organizer-bar-chart.css']
})
export class OrganizerBarChartComponent {
  selectedPeriod = 'Last 7 days';
  
  // Sample data based on the image description
  chartData = [
    { label: 'Group 1', organizer: 410, attendee: 320 },
    { label: 'Group 2', organizer: 520, attendee: 600 },
    { label: 'Group 3', organizer: 500, attendee: 320 },
    { label: 'Group 4', organizer: 560, attendee: 640 },
    { label: 'Group 5', organizer: 510, attendee: 320 },
    { label: 'Group 6', organizer: 510, attendee: 560 },
    { label: 'Group 7', organizer: 440, attendee: 240 },
    { label: 'Group 8', organizer: 260, attendee: 320 },
    { label: 'Group 9', organizer: 380, attendee: 280 },
    { label: 'Group 10', organizer: 540, attendee: 440 }
  ];

  get maxValue() {
    return Math.max(
      ...this.chartData.flatMap(d => [d.organizer, d.attendee])
    );
  }

  getBarHeight(value: number): string {
    return `${(value / this.maxValue) * 100}%`;
  }
}

