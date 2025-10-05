import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Team } from '../../models/Team';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  accessToken: string | null = null;
  refreshToken: string | null = null;
  private teamSubject = new BehaviorSubject<Team | null>(null);
  team$ = this.teamSubject.asObservable();

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

  setTeam(team: Team) {
    this.teamSubject.next(team);
  }

  getTeam(): Team | null {
    return this.teamSubject.value;
  }
}

