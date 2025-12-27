import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bulk-actions',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bulk-actions.html',
  styleUrls: ['./bulk-actions.css']
})
export class BulkActionsComponent {
  hasSelection = false;

  onSelectionChange(selected: boolean) {
    this.hasSelection = selected;
  }
}

