import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './signin.html',
  styleUrls: ['./signin.css']
})
export class SigninComponent {
  selectedRole: string = 'organizer';

  constructor(private router: Router) {}

  onSubmit() {
    // Route based on selected role
    switch(this.selectedRole) {
      case 'admin':
        this.router.navigate(['/admin/dashboard']);
        break;
      case 'organizer':
        this.router.navigate(['/organizer/dashboard']);
        break;
      case 'attendee':
        this.router.navigate(['/attendee']);
        break;
      default:
        this.router.navigate(['/organizer/dashboard']);
    }
  }
}
