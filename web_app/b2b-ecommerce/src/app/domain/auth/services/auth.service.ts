import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../core/services/base.service';
import { catchError, map, Observable } from 'rxjs';
import { AuthResponse, RegisterRequest } from '../models/auth.models';
import { environment } from '../../../../environments/environment';


@Injectable({ providedIn: 'root' })
export class AuthService extends BaseService {
  private http = inject(HttpClient);
  private readonly API_URL = `${environment.apis.auth}/auth`;

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.API_URL}/register`, data, this.httpOptions)
      .pipe(
        map(this.extractData),
        catchError(this.serviceError) // Usa o tratamento da BaseService
      );
  }

  login(credentials: any): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.API_URL}/login`, credentials, this.httpOptions)
      .pipe(
        map(this.extractData),
        catchError(this.serviceError)
      );
  }
}
