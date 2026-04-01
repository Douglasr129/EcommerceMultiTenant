import { Routes } from '@angular/router';

export const ROUTES: Routes = [
  {
    path:'',
    loadComponent: ()=> import('./app').then((m)=> m.App),
  }
];
