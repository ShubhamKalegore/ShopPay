import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  submitted = false;
  errorMessage: string | null = null;
  isSubmitting = false;
  private readonly loginUrl = 'https://localhost:9001/api/users/login';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessage = null;

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const payload = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };

    this.isSubmitting = true;
    this.authService.login(payload).subscribe({
      next: (user) => {
        localStorage.setItem('shopPayUser', JSON.stringify(user));
        this.isSubmitting = false;
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.errorMessage = 'Invalid email or password.';
        this.isSubmitting = false;
      }
    });
  }
}
