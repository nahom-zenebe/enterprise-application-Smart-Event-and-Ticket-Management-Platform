import { Routes } from '@angular/router';

export const attendeeRoutes: Routes = [
  {
    path: 'events',
    loadComponent: () =>
      import('./event-list/event-list.component')
        .then(m => m.EventListComponent)
  },
  {
    path: 'events/:id',
    loadComponent: () =>
      import('./event-details/event-details.component')
        .then(m => m.EventDetailsComponent)
  },
  {
    path: 'checkout/:id',
    loadComponent: () =>
      import('./checkout/checkout.component')
        .then(m => m.CheckoutComponent)
  },
  {
    path: 'my-tickets',
    loadComponent: () =>
      import('./my-tickets/my-tickets.component')
        .then(m => m.MyTicketsComponent)
  },
  {
    path: 'ticket/:id',
    loadComponent: () =>
      import('./ticket-details/ticket-details.component')
        .then(m => m.TicketDetailsComponent)
  }
];
