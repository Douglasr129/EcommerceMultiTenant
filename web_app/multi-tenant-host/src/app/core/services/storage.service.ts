import { inject, Injectable } from '@angular/core';
import { StorageKeys } from '../constants/storage.constants';
import { CookieService } from 'ngx-cookie-service';

@Injectable({ providedIn: 'root' })
export class StorageService {
  private cookieService = inject(CookieService);
  setObject(key: StorageKeys, value: any): void {
    const strValue = JSON.stringify(value);
    this.cookieService.set(key, strValue, { expires: 7, path: '/' });
  }
  getObject<T>(key: StorageKeys): T | null {
    const value = this.cookieService.get(key);
    if (!value) return null;
    try {
      const decoded = value.startsWith('%') ? decodeURIComponent(value) : value;
      return (JSON.parse(decoded) as T) || null;
    } catch (e) {
      console.error('Erro ao fazer parse do Cookie:', e);
      return null;
    }
  }
  setRaw(key: StorageKeys, value: string): void {
    this.cookieService.set(key, value, { expires: 7, path: '/' });
  }
  getRaw(key: StorageKeys): string | null {
    return this.cookieService.get(key) || null;
  }
  deleteCookie(key: StorageKeys | string): void {
    this.cookieService.delete(key, '/');
  }
  deleteAll(): void {
    this.cookieService.deleteAll('/');
    if (typeof window !== 'undefined') {
      localStorage.clear();
    }
  }
}
