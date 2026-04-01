import { inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { ToastService } from '../services/toast.service';
import { StorageService } from '../services/storage.service';
import { StorageKeys } from '../constants/storage.constants';
import { UserPayload } from '../interfaces/user-payload.interface';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(
    private storage: StorageService,
    private toast: ToastService,
    private router: Router,
  ) {}
  canActivate(route: ActivatedRouteSnapshot) {
    const cookieData = this.storage.getObject<UserPayload>(StorageKeys.AUTH_USER);
    if (!cookieData) {
      this.router.navigate(['/']);
      return false;
    }
    try {
      const user = cookieData;
      const userRole = user?.role;
      const expectedRoles = route.data['roles'] as Array<string>;
      if (!user) {
        this.router.navigate(['/']);
        return false;
      }
      if (!expectedRoles || expectedRoles.length === 0) {
        return true;
      }
      if (expectedRoles.includes(userRole)) {
        return true;
      }
      this.toast.showError('Você não tem permissão para acessar esta área.');
      switch (user.role) {
        case 'SELLER':
          this.router.navigate(['/seller/dashboard']);
          break;
        default:
          this.router.navigate(['/client/dashboard']);
          break;
      }
      return false;
    } catch (error) {
      console.error('Erro ao ler cookie no Guard:', error);
      this.router.navigate(['/auth']);
      return false;
    }
  }
}
