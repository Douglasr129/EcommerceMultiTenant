import { computed, inject, Injectable, signal } from '@angular/core';
import { BaseService } from './base.service';
import { catchError, map, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { UserPayload } from '../interfaces/user-payload.interface';
import { StorageKeys } from '../constants/storage.constants'; // Importe seu Enum

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseService {
  private router = inject(Router);

  public currentUser = computed(() => this.userSignal());
  public isAuthenticated = computed(() => !!this.userSignal());
  constructor() {
    super();
  }
 private getInitialUser(): UserPayload | null {
  try {
    const data = this.storage.getObject<UserPayload>(StorageKeys.AUTH_USER);
    if (!data || data === null) return null;
    return data;
  } catch (e) {
    return null;
  }
}
  private userSignal = signal<UserPayload | null>(this.getInitialUser());
  login(credentials: any): Observable<any> {
    return this.http
      .post(`${this.urlBase}/auth/login`, credentials, this.httpOptions)
      .pipe(
        map(this.extractData),
        tap((res: any) => this.saveSession(res)),
        catchError((err) => this.serviceError(err)),
      );
  }
  register(userData: any): Observable<any> {
    return this.http
      .post(`${this.urlBase}/auth/register`, userData, this.httpOptions)
      .pipe(
        map(this.extractData),
        catchError((err) => this.serviceError(err)),
      );
  }
  hasRole(expectedRoles: string[]): boolean {
    const user = this.userSignal();
    return user
      ? expectedRoles
          .map((r) => r.toLowerCase())
          .includes(user.role.toLowerCase())
      : false;
  }
  getUserRole(): string | null {
    return this.userSignal()?.role || null;
  }
  isLoggedIn(): boolean {
    return this.isAuthenticated();
  }
  logout(): void {
    this.storage.deleteCookie(StorageKeys.AUTH_TOKEN);
    this.storage.deleteCookie(StorageKeys.AUTH_USER);
    this.userSignal.set(null);
    this.router.navigate(['/']);
  }
  private saveSession(res: any): void {
    const user = res.user || res.data?.user;
    const token = res.token || res.user?.token || res.data?.token;

    if (token && user) {
      this.storage.setRaw(StorageKeys.AUTH_TOKEN, token);
      this.storage.setObject(StorageKeys.AUTH_USER, user);
      this.userSignal.set(user);
    } else {
      console.error('Erro no login: Estrutura de resposta inválida', res);
    }
  }
}
