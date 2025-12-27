import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { DashboardModule } from './dashboard/dashboard.module';
import { EventsModule } from './events/events.module';
import { EventEditorModule } from './event-editor/event-editor.module';
import { OrdersModule } from './orders/orders.module';
import { SettingsModule } from './settings/settings.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    DashboardModule,
    EventsModule,
    EventEditorModule,
    OrdersModule,
    SettingsModule
  ]
})
export class OrganizerModule {}

