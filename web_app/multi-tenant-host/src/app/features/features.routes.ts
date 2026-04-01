import { loadRemoteModule } from '@angular-architects/module-federation';
import { Routes } from '@angular/router';
import { Features } from './features';

export const FEATURES_ROUTE: Routes = [
  {
    path: 'catalog',
    component: Features,
    loadChildren: () =>
      loadRemoteModule({
        type: 'module',
        remoteEntry: 'http://localhost:4201/remoteEntry.js',
        exposedModule: './Routes'
      }).then(m => m.ROUTES)
  }
];
