import { Routes } from '@angular/router';


export const PAGES_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: '', // Rota vazia (Home)
        loadComponent: () => import('./home/home').then(m => m.Home)
      },
      {
        path: 'about', // CORRETO: 'about' em vez de '/about'
        loadComponent: () => import('./about/about').then(m => m.About)
      },
      {
        path: 'contact', // CORRETO: 'login' em vez de '/login'
        loadComponent: () => import('./contact/contact').then(m => m.Contact)
      }
    ]
  }
];
