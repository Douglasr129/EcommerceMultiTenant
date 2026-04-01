import { Routes } from '@angular/router';
import { Auth } from './pages/auth/auth';


export const AUTH_ROUTES: Routes = [
  { path: '', component: Auth },
  { path: '**', redirectTo: '', pathMatch: 'full' }
]
