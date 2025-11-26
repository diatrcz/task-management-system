import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ApiService, UserModel, TeamSummaryDto } from '../../../services/api-service.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  standalone: false
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  selectedTeamIds: string[] = [];
  teams: TeamSummaryDto[] = [];
  errorMessage: string = '';
  isLoading: boolean = false;
  teamSelectionTouched: boolean = false;

  constructor(
    private apiService: ApiService,
    private router: Router,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.initializeForm();
    this.loadTeams();
  }

  initializeForm() {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (!password || !confirmPassword) {
      return null;
    }

    return password.value === confirmPassword.value ? null : { passwordMismatch: true };
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

  getValidationErrors(): string[] {
    const errors: string[] = [];

    if (this.registerForm.get('email')?.invalid && this.registerForm.get('email')?.touched) {
      if (this.registerForm.get('email')?.errors?.['required']) {
        errors.push('Email is required');
      }
      if (this.registerForm.get('email')?.errors?.['email']) {
        errors.push('Please enter a valid email address');
      }
    }

    if (this.registerForm.get('password')?.invalid && this.registerForm.get('password')?.touched) {
      if (this.registerForm.get('password')?.errors?.['required']) {
        errors.push('Password is required');
      }
      if (this.registerForm.get('password')?.errors?.['minLength']) {
        errors.push('Password must be at least 6 characters');
      }
    }

    if (this.registerForm.get('confirmPassword')?.invalid && this.registerForm.get('confirmPassword')?.touched) {
      if (this.registerForm.get('confirmPassword')?.errors?.['required']) {
        errors.push('Please confirm your password');
      }
    }

    if (this.registerForm.errors?.['passwordMismatch'] && this.registerForm.get('confirmPassword')?.touched) {
      errors.push('Passwords do not match');
    }

    if (this.registerForm.get('firstName')?.invalid && this.registerForm.get('firstName')?.touched) {
      errors.push('First name is required');
    }

    if (this.registerForm.get('lastName')?.invalid && this.registerForm.get('lastName')?.touched) {
      errors.push('Last name is required');
    }

    if (this.teamSelectionTouched && this.selectedTeamIds.length === 0) {
      errors.push('Please select at least one team');
    }

    return errors;
  }

  register() {
    this.errorMessage = '';

    Object.keys(this.registerForm.controls).forEach(key => {
      this.registerForm.get(key)?.markAsTouched();
    });

    this.teamSelectionTouched = true;

    if (this.registerForm.invalid || this.selectedTeamIds.length === 0) {
      this.errorMessage = 'Please fill out all required fields correctly';
      return;
    }

    this.isLoading = true;

    const model = new UserModel({
      email: this.registerForm.value.email.trim(),
      password: this.registerForm.value.password,
      firstName: this.registerForm.value.firstName.trim(),
      lastName: this.registerForm.value.lastName.trim(),
      teamIds: this.selectedTeamIds
    });

    this.apiService.user_Register(model).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.success) {
          this.router.navigate(['/userlogin']);
        } else {
          this.errorMessage = response.message || 'Registration failed.';
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