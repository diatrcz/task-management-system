import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService, UserModel, TeamSummaryDto } from '../../../services/api-service.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    standalone: false
})
export class RegisterComponent implements OnInit {
  email: string = '';
  password: string = '';
  firstName: string = '';
  lastName: string = '';
  selectedTeamIds: string[] = [];
  teams: TeamSummaryDto[] = [];
  errorMessage: string = '';
  validationErrors: string[] = [];
  isLoading: boolean = false;

  constructor(private apiService: ApiService, private router: Router) {}

  ngOnInit() {
    this.loadTeams();
  }

  loadTeams() {
    this.apiService.user_GetTeams().subscribe({
      next: (teams) => {
        this.teams = teams;
      },
      error: (err) => {
        console.error('Failed to load teams:', err);
        this.errorMessage = 'Failed to load teams. Please refresh the page.';
      }
    });
  }

  onTeamSelectionChange(teamId: string, isChecked: boolean) {
    if (isChecked) {
      if (!this.selectedTeamIds.includes(teamId)) {
        this.selectedTeamIds.push(teamId);
      }
    } else {
      this.selectedTeamIds = this.selectedTeamIds.filter(id => id !== teamId);
    }
  }

  isTeamSelected(teamId: string): boolean {
    return this.selectedTeamIds.includes(teamId);
  }

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

    if (!this.firstName || this.firstName.trim() === '') {
      this.validationErrors.push('First name is required');
    }

    if (!this.lastName || this.lastName.trim() === '') {
      this.validationErrors.push('Last name is required');
    }

    if (this.selectedTeamIds.length === 0) {
      this.validationErrors.push('Please select at least one team');
    }

    return this.validationErrors.length === 0;
  }

  isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  register() {
    this.errorMessage = '';
    this.validationErrors = [];

    if (!this.validateForm()) {
      this.errorMessage = 'Please fill out all required fields';
      return;
    }

    this.isLoading = true;

    const model = new UserModel({
      email: this.email.trim(),
      password: this.password,
      firstName: this.firstName.trim(),
      lastName: this.lastName.trim(),
      teamIds: this.selectedTeamIds
    });

    this.apiService.user_Register(model).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.success) {
          this.router.navigate(['/userlogin']);
        } else {
          this.errorMessage = response.message || 'Registration failed.';
          if (response.errors && response.errors.length > 0) {
            this.validationErrors = response.errors;
          }
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Bad request. Registration failed. Please check your information and try again.';
        console.error('Registration error:', err);
      }
    });
  }
}