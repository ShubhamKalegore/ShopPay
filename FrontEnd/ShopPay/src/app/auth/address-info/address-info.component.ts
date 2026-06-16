import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-address-info',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './address-info.component.html',
  styleUrls: ['./address-info.component.scss']
})
export class AddressInfoComponent {
  addressForm: FormGroup;
  submitted = false;
  isSubmitting = false;

  @Output() saveAddress = new EventEmitter<any>();

  constructor(private fb: FormBuilder) {
    this.addressForm = this.fb.group({
      address_type: ['', Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postal_code: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{5}(-\d{4})?$/)
        ]
      ],
      country: ['', Validators.required],
      is_default: [false]
    });
  }

  onRegister() {
    this.submitted = true;

    if (this.addressForm.valid) {
      this.saveAddress.emit(this.addressForm.value);
    } else {
      this.addressForm.markAllAsTouched();
    }
  }
}