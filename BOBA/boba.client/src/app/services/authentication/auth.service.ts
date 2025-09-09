import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  accessToken: string | null = null;
  refreshToken: string | null = null;

  constructor(private http: HttpClient) { }

  setLoggedInUser(accessToken: string, refreshToken: string) {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
  }

  clearLoggedInUser() {
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

