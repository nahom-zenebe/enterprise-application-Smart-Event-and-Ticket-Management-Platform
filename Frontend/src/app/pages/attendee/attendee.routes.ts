import { Routes } from '@angular/router';
import { EventListComponent } from './event-list/event-list.component';
import { EventDetailsComponent } from './event-details/event-details.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { MyTicketsComponent } from './my-tickets/my-tickets.component';
import { TicketDetailsComponent } from './ticket-details/ticket-details.component';

export const attendeeRoutes: Routes = [
  { path: 'events', component: EventListComponent },
  { path: 'events/:id', component: EventDetailsComponent },
  { path: 'checkout/:id', component: CheckoutComponent },
  { path: 'my-tickets', component: MyTicketsComponent },
  { path: 'ticket/:id', component: TicketDetailsComponent }
];
