import { Routes } from '@angular/router';
import { SigninComponent } from './pages/signin/signin';
import { RegisterComponent } from './pages/register/register';
import { HomeComponent } from './pages/home/home';
import { OrganizerLayoutComponent } from './pages/organizer/organizer-layout/organizer-layout.component';
import { DashboardComponent } from './pages/organizer/dashboard/dashboard.component';
import { EventsComponent } from './pages/organizer/events/events.component';
import { EventEditorComponent } from './pages/organizer/event-editor/event-editor.component';
import { OrdersComponent } from './pages/organizer/orders/orders.component';
import { SettingsComponent } from './pages/organizer/settings/settings.component';
import { AdminLayoutComponent } from './pages/admin/admin-layout/admin-layout.component';
import { AdminDashboardComponent } from './pages/admin/dashboard/dashboard.component';
import { AdminEventsComponent } from './pages/admin/events/events.component';
import { AdminUsersComponent } from './pages/admin/users/users.component';
import { AttendeeComponent } from './pages/attendee/attendee.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'signin', component: SigninComponent },
  { path: 'register', component: RegisterComponent },
  { 
    path: 'organizer', 
    component: OrganizerLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'events', component: EventsComponent },
      { path: 'event-editor', component: EventEditorComponent },
      { path: 'orders', component: OrdersComponent },
      { path: 'settings', component: SettingsComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { 
    path: 'admin', 
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'events', component: AdminEventsComponent },
      { path: 'users', component: AdminUsersComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: 'attendee', component: AttendeeComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' } // default route
];
