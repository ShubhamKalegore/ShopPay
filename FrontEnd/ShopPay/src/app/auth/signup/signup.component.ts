import { Component } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
  imports: [ReactiveFormsModule,CommonModule],
  standalone: true, // Assuming this is a standalone component
})
export class SignupComponent {
  signupForm: FormGroup;
  submitted = false;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  showAddressForm = false; // <-- New flag to toggle form steps
  isSubmitting = false;
  private readonly registerUrl = 'https://localhost:9000/api/users/register';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.signupForm = this.fb.group(
      {
        firstName: ['', [Validators.required, Validators.minLength(3)]],
        lastName: ['', [Validators.required, Validators.minLength(3)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required],
        address_type: ['', Validators.required],
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        postal_code: ['', [Validators.required, Validators.pattern(/^\d{5}(-\d{4})?$/)]],
        country: ['', Validators.required],
        is_default: [false],
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

  onNext() {
    this.submitted = true;

    // Validate only user info and password controls before proceeding
    const userInfoValid =
      this.signupForm.get('firstName')?.valid &&
      this.signupForm.get('lastName')?.valid &&
      this.signupForm.get('email')?.valid &&
      this.signupForm.get('password')?.valid &&
      this.signupForm.get('confirmPassword')?.valid &&
      !this.signupForm.errors?.['mismatch'];

    if (userInfoValid) {
      this.showAddressForm = true;
      this.submitted = false; // reset for next step
    } else {
      this.signupForm.get('firstName')?.markAsTouched();
      this.signupForm.get('lastName')?.markAsTouched();
      this.signupForm.get('email')?.markAsTouched();
      this.signupForm.get('password')?.markAsTouched();
      this.signupForm.get('confirmPassword')?.markAsTouched();
    }
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
          this.signupForm.reset({ is_default: false });
          this.showAddressForm = false;
          this.submitted = false;
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
