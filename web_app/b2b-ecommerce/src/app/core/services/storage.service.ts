import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  // --- LOCAL STORAGE (Cache de longa duração) ---

  set(key: string, value: any): void {
    const data = typeof value === 'string' ? value : JSON.stringify(value);
    localStorage.setItem(key, data);
  }

  get<T>(key: string): T | null {
    const data = localStorage.getItem(key);
    if (!data) return null;

    try {
      return JSON.parse(data) as T;
    } catch {
      return data as unknown as T;
    }
  }

  remove(key: string): void {
    localStorage.removeItem(key);
  }

  clear(): void {
    localStorage.clear();
  }

  // --- COOKIES (Segurança e Token) ---
  // Nota: Cookies são ideais para Tokens que precisam ser lidos pelo servidor (SSR)

  setCookie(name: string, value: string, days: number = 7): void {
    const date = new Date();
    date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
    const expires = "; expires=" + date.toUTCString();
    document.cookie = name + "=" + (value || "") + expires + "; path=/; SameSite=Lax";
  }

  getCookie(name: string): string | null {
    const nameEQ = name + "=";
    const ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
      let c = ca[i];
      while (c.charAt(0) === ' ') c = c.substring(1, c.length);
      if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
  }

  deleteCookie(name: string): void {
    document.cookie = name + '=; Max-Age=-99999999;';
  }
}
