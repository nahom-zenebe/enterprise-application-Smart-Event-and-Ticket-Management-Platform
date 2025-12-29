import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AttendeeService } from '../attendee.services';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-tickets.component.html',
  styleUrls: ['./my-tickets.component.css']
})
export class MyTicketsComponent implements OnInit {

  tickets: any[] = [];

  constructor(
    private service: AttendeeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.tickets = this.service.getMyTickets();
  }

  viewTicket(id: number): void {
    this.router.navigate(['/attendee/ticket', id]);
  }
}
