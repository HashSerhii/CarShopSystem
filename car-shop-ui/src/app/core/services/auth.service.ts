import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { AuthResponse } from '../models/api.models';

const TOKEN_KEY = 'carshop_token';

interface JwtPayload {
  sub?: string;
  email?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
  role?: string | string[];
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenSignal = signal<string | null>(this.readToken());

  readonly isLoggedIn = computed(() => !!this.tokenSignal());
  readonly userId = computed(() => this.parseToken()?.sub ?? null);
  readonly email = computed(() => this.parseToken()?.email ?? null);
  readonly isAdmin = computed(() => {
    const roles = this.getRoles();
    return roles.includes('Admin');
  });

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {}

  login(email: string, password: string) {
    return this.http
      .post<AuthResponse>('/api/users/login', { email, password })
      .pipe(tap((res) => this.setToken(res.token)));
  }

  register(email: string, password: string, phoneNumber: string) {
    return this.http.post('/api/users/register', {
      email,
      password,
      phoneNumber,
    });
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    this.tokenSignal.set(null);
    void this.router.navigate(['/']);
  }

  getToken(): string | null {
    return this.tokenSignal();
  }

  private setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
    this.tokenSignal.set(token);
  }

  private readToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private parseToken(): JwtPayload | null {
    const token = this.tokenSignal();
    if (!token) return null;
    try {
      const payload = token.split('.')[1];
      const json = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
      return JSON.parse(json) as JwtPayload;
    } catch {
      return null;
    }
  }

  private getRoles(): string[] {
    const p = this.parseToken();
    if (!p) return [];
    const raw =
      p['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ??
      p.role;
    if (!raw) return [];
    return Array.isArray(raw) ? raw : [raw];
  }
}
