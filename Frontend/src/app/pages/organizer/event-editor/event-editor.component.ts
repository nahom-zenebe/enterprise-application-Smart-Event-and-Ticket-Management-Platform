import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasicInfoFormComponent } from './components/basic-info-form/basic-info-form.component';
import { ScheduleVenueFormComponent } from './components/schedule-venue-form/schedule-venue-form.component';
import { TicketsPricingFormComponent } from './components/tickets-pricing-form/tickets-pricing-form.component';
import { ReviewPublishComponent } from './components/review-publish/review-publish.component';

@Component({
  selector: 'app-event-editor',
  standalone: true,
  imports: [CommonModule, BasicInfoFormComponent, ScheduleVenueFormComponent, TicketsPricingFormComponent, ReviewPublishComponent],
  templateUrl: './event-editor.html',
  styleUrls: ['./event-editor.css']
})
export class EventEditorComponent {
  currentStep: number = 1;
  totalSteps: number = 4;

  steps = [
    { number: 1, title: 'Basic Info', component: 'basic-info' },
    { number: 2, title: 'Schedule & Venue', component: 'schedule-venue' },
    { number: 3, title: 'Tickets & Pricing', component: 'tickets-pricing' },
    { number: 4, title: 'Review & Publish', component: 'review-publish' }
  ];

  nextStep() {
    if (this.currentStep < this.totalSteps) {
      this.currentStep++;
    }
  }

  previousStep() {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  goToStep(step: number) {
    if (step >= 1 && step <= this.totalSteps) {
      this.currentStep = step;
    }
  }
}

