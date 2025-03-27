import { Component } from '@angular/core';
import { AuthService } from '../../../services/authentication/auth.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private userService: UserService, private authService: AuthService, private router: Router) {}

  login() {
    this.userService.login({ email: this.email, password: this.password }).subscribe({
      next: (response) => {
        this.authService.setLoggedInUser(response.userId, response.accessToken, response.refreshToken);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.errorMessage = 'Invalid credentials. Please try again.';
      }
    });
  }

}
