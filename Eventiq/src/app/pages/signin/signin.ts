import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './signin.html',
  styleUrls: ['./signin.css']
})
export class SigninComponent {}
