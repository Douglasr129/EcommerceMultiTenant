import { Routes } from '@angular/router';
import { Dashboard } from './dashboard/dashboard';

export const CLIENT_ROUTES: Routes = [
  { path: '', component: Dashboard },
  { path: '**', redirectTo: '', pathMatch: 'full' }
]
