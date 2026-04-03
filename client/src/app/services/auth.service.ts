import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

interface AuthResponse {
  token: string;
}

interface LoginRequest {
  username: string;
  password: string;
}

interface RegisterRequest {
  username: string;
  password: string;
}

// Base URL for auth API
const API = 'http://localhost:5268/api/auth';
// Key used to store the token in localStorage
const TOKEN_KEY = 'jwt_token';

// Handles login, register, logout and token storage
@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  // Load saved token from localStorage on start
  private tokenSignal = signal(localStorage.getItem(TOKEN_KEY));

  // True if user has a token
  isLoggedIn = computed(() => !!this.tokenSignal());

  get token(): string | null {
    return this.tokenSignal();
  }

  // Send register request to API
  register(data: RegisterRequest) {
    return this.http.post(`${API}/register`, data);
  }

  // Send login request, save token on success
  login(data: LoginRequest) {
    return this.http.post<AuthResponse>(`${API}/login`, data).pipe(
      tap((res) => {
        localStorage.setItem(TOKEN_KEY, res.token);
        this.tokenSignal.set(res.token);
      })
    );
  }

  // Remove token and go to login page
  logout() {
    localStorage.removeItem(TOKEN_KEY);
    this.tokenSignal.set(null);
    this.router.navigate(['/login']);
  }
}
