import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userId: string | null = null;
  accessToken: string | null = null;
  refreshToken: string | null = null;

  constructor(private http: HttpClient) { }

  setLoggedInUser(userId: string, accessToken: string, refreshToken: string) {
    this.userId = userId;
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
  }

  clearLoggedInUser() {
    this.userId = null;
    this.accessToken = null;
    this.refreshToken = null;
  }

  getToken(): string | null {
    return this.accessToken;
  }

  isLoggedIn(): boolean {
    if (!this.accessToken) {
      return false;
    }

    return true;
  }
}

