import { Component } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  standalone: true, // Assuming this is a standalone component
})
export class SignupComponent {
  signupForm: FormGroup;
  submitted = false;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  showAddressForm = false; // <-- New flag to toggle form steps
  isSubmitting = false;
  private readonly registerUrl = 'https://localhost:9001/api/users/register';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.signupForm = this.fb.group(
    {
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmPassword: ['', Validators.required]
    },
    {
      validators: [this.passwordMatchValidator]
    } as AbstractControlOptions
    );
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
    this.submitted = true;
    this.successMessage = null;
    this.errorMessage = null;

    if (this.signupForm.valid) {
      const formValue = this.signupForm.value;
      const payload = {
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        email: formValue.email,
        password: formValue.password
      };

      this.isSubmitting = true;
      this.http.post(this.registerUrl, payload).subscribe({
        next: () => {
          this.successMessage = 'Registration successful!';
          this.signupForm.reset();
          this.isSubmitting = false;
          this.router.navigate(['/login']);
        },
        error: () => {
          this.errorMessage = 'Registration failed. Please check the backend and try again.';
          this.isSubmitting = false;
        }
      });
    } else {
      this.errorMessage = 'Please fill out all required fields correctly.';
      this.signupForm.markAllAsTouched();
    }
  }
}
