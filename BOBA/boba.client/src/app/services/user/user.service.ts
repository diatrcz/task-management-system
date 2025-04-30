import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../authentication/auth.service';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private authService: AuthService) {}

  register(userData: { username: string; email: string; password: string }): Observable<any> {
    return this.http.post(`/register`, userData);
  }

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post(`/login`, credentials);
  }

  logout(): void {
    this.authService.clearLoggedInUser();
  }

  getUserNameById(userId: string) {
    return this.http.get<string>('api/User/name-by-id', {
      params: { userId }
    });
  }

}
