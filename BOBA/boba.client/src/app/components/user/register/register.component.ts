import { Component } from '@angular/core';
import { UserService } from '../../../services/user/user.service';
import { Router } from '@angular/router';

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

  constructor(private userService: UserService, private router: Router) {}

  register() {
    const userData = { username: this.username, email: this.email, password: this.password };
    this.userService.register(userData).subscribe({
      next: (response) => {
        this.router.navigate(['/userlogin']);
        console.log('success');
      },
      error: (err) => {
        this.errorMessage = 'Registration failed. Please try again.';
        console.log('wompwomp');
      }
    });
  }

}
