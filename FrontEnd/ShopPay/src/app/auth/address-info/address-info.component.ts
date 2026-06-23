import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators
} from '@angular/forms';
import { AddressService, CreateAddressPayload } from '../../core/services/address.service';

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
    errorMessage: string | null = null;

    @Output() saveAddress = new EventEmitter<any>();

    constructor(
        private fb: FormBuilder,
        private addressService: AddressService
    ) {
        this.addressForm = this.fb.group({
            address_type: ['', Validators.required],
            street: ['', Validators.required],
            city: ['', Validators.required],
            state: ['', Validators.required],
            postal_code: [
                '',
                [
                    Validators.required,
                    Validators.pattern(/^(\d{5}(-\d{4})?|\d{6})$/)
                ]
            ],
            country: ['', Validators.required],
            is_default: [false]
        });
    }

    onRegister() {
        this.submitted = true;
        this.errorMessage = null;

        if (this.addressForm.valid) {
            const userData = localStorage.getItem('shopPayUser');
            const user = userData ? JSON.parse(userData) : null;

            if (!user?.userId) {
                this.errorMessage = 'Please log in before adding an address.';
                return;
            }

            const formValue = this.addressForm.value;
            const payload: CreateAddressPayload = {
                userId: user.userId,
                addressType: formValue.address_type,
                street: formValue.street,
                city: formValue.city,
                state: formValue.state,
                postalCode: formValue.postal_code,
                country: formValue.country,
                isDefault: formValue.is_default
            };

            this.isSubmitting = true;
            this.addressService.createAddress(payload).subscribe({
                next: (response) => {
                    const updatedUser = {
                        ...user,
                        isAddressPresent: true,
                        postalCode: payload.postalCode
                    };

                    localStorage.setItem('shopPayUser', JSON.stringify(updatedUser));
                    this.isSubmitting = false;
                    this.saveAddress.emit({
                        response,
                        postalCode: payload.postalCode,
                        updatedUser
                    });
                },
                error: () => {
                    this.isSubmitting = false;
                    this.errorMessage = 'Unable to save address. Please try again.';
                }
            });

        } else {

            this.addressForm.markAllAsTouched();
        }
    }
}
