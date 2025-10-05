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
  email!: string;
  password!: string;
  errorMessage: string = '';

  constructor(private apiService: ApiService, private authService: AuthService, private router: Router) {}

  login() {
  const request = new LoginRequest({
    email: this.email,
    password: this.password
  });

  this.apiService.postLogin(request, false, false).subscribe({
    next: (response) => {
      if(response.accessToken && response.refreshToken)
        this.authService.setLoggedInUser(response.accessToken, response.refreshToken);
      this.router.navigate(['/dashboard']);
    },
    error: () => {
      this.errorMessage = 'Invalid credentials. Please try again.';
    }
  });
}


}
