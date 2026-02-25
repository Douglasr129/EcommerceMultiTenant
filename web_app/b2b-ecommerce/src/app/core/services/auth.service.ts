import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private token: string | null = null;
  private role: string | null = null;

  getRole(): string {
      if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
        return localStorage.getItem('role') || 'Customer';
      }
      return 'Customer';
  }

  login(token: string, role: string) {
    this.token = token;
    this.role = role;
    localStorage.setItem('token', token);
    localStorage.setItem('role', role);
  }

  logout() {
    this.token = null;
    this.role = null;
    localStorage.removeItem('token');
    localStorage.removeItem('role');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserRole(): string {
    return localStorage.getItem('role') || '';
  }
}
