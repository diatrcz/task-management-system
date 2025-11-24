import { Component } from '@angular/core';
import { AuthService } from '../../../services/authentication/auth.service';
import { Router } from '@angular/router';
import { ApiService, LoginRequest } from '../../../services/api-service.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.css',
    standalone: false
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  validationErrors: string[] = [];
  isLoading: boolean = false;

  constructor(private apiService: ApiService, private authService: AuthService, private router: Router) {}

  validateForm(): boolean {
    this.validationErrors = [];

    if (!this.email || this.email.trim() === '') {
      this.validationErrors.push('Email is required');
    } else if (!this.isValidEmail(this.email)) {
      this.validationErrors.push('Please enter a valid email address');
    }

    if (!this.password || this.password.trim() === '') {
      this.validationErrors.push('Password is required');
    }

    return this.validationErrors.length === 0;
  }

  isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  login() {
    this.errorMessage = '';
    this.validationErrors = [];

    if (!this.validateForm()) {
      this.errorMessage = 'Please fill out all required fields';
      return;
    }

    this.isLoading = true;

    const request = new LoginRequest({
      email: this.email.trim(),
      password: this.password
    });

    this.apiService.postLogin(request, false, false).subscribe({
      next: (response) => {
        this.isLoading = false;
        if(response.accessToken && response.refreshToken) {
          this.authService.setLoggedInUser(response.accessToken, response.refreshToken);
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = 'Login failed. Invalid response from server.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Invalid credentials. Please try again.';
        console.error('Login error:', err);
      }
    });
  }
}