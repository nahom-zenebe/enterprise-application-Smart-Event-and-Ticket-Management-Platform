import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EventsTableComponent } from './components/events-table/events-table.component';
import { EventsGridComponent } from './components/events-grid/events-grid.component';
import { BulkActionsComponent } from './components/bulk-actions/bulk-actions.component';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [CommonModule, RouterModule, EventsTableComponent, EventsGridComponent, BulkActionsComponent],
  templateUrl: './events.html',
  styleUrls: ['./events.css']
})
export class EventsComponent {}

