import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService, RegisterRequest } from '../../../services/api-service.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    standalone: false
})
export class RegisterComponent {
  username: string = '';
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private apiService: ApiService, private router: Router) {}

  register() {
    const request = new RegisterRequest ({ email: this.email, password: this.password });
    this.apiService.postRegister(request).subscribe({
      next: (response) => {
        this.router.navigate(['/user  login']);
        console.log('success');
      },
      error: (err) => {
        this.errorMessage = 'Registration failed. Please try again.';
        console.log('wompwomp');
      }
    });
  }

}
