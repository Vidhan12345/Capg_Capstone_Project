import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { LoginRequest, LoginResponse, LoggedInUser, ChangePasswordRequest } from '../models/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly TOKEN_KEY = 'wms_token';
  private readonly USER_KEY = 'wms_user';

  private currentUserSubject = new BehaviorSubject<LoggedInUser | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();
  isLoggedIn$ = this.currentUser$.pipe(map(u => u !== null));
  currentRole$ = this.currentUser$.pipe(map(u => u?.role ?? null));

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      const stored = localStorage.getItem(this.USER_KEY);
      if (stored) {
        try {
          const user: LoggedInUser = JSON.parse(stored);
          this.currentUserSubject.next(user);
        } catch { this.clearAuth(); }
      }
    }
  }

  login(credentials: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(`${environment.apiUrl}/Auth/login`, credentials).pipe(
      tap(res => {
        if (res.success && res.data) {
          const user: LoggedInUser = {
            employeeId: res.data.employeeId,
            username: credentials.username,
            email: '',
            role: res.data.role,
            employeeCode: '',
            employeeName: res.data.employeeName,
            token: res.data.token
          };
          this.setAuth(user, res.data.token);
        }
      })
    );
  }

  changePassword(dto: ChangePasswordRequest): Observable<ApiResponse<object>> {
    return this.http.post<ApiResponse<object>>(`${environment.apiUrl}/Auth/change-password`, dto);
  }

  logout(): void {
    const token = this.getToken();
    if (token) {
      this.http.post(`${environment.apiUrl}/Auth/logout`, {}, {
        headers: { Authorization: `Bearer ${token}` }
      }).subscribe({ error: () => {} });
    }
    this.clearAuth();
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) return null;
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getUser(): LoggedInUser | null {
    return this.currentUserSubject.value;
  }

  hasRole(...roles: string[]): boolean {
    const user = this.getUser();
    return !!user && roles.includes(user.role);
  }

  private setAuth(user: LoggedInUser, token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.TOKEN_KEY, token);
      localStorage.setItem(this.USER_KEY, JSON.stringify(user));
    }
    this.currentUserSubject.next(user);
  }

  private clearAuth(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.TOKEN_KEY);
      localStorage.removeItem(this.USER_KEY);
    }
    this.currentUserSubject.next(null);
  }
}
