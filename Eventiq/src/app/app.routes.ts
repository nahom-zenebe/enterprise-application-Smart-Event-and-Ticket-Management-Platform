import { Routes } from '@angular/router';
import { SigninComponent } from './pages/signin/signin';
import { RegisterComponent } from './pages/register/register';
import { HomeComponent } from './pages/home/home';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'signin', component: SigninComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' } // default route
];
