import {
  Component,
  EventEmitter,
  Output,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import {
  CreateProductPayload,
  ProductService
} from '../../core/services/product.service';

@Component({
  selector: 'app-product-info',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.scss'
})
export class ProductInfoComponent {

  private readonly fb = inject(FormBuilder);
  private readonly productService = inject(ProductService);

  @Output()
  saveProduct = new EventEmitter<any>();

  productForm: FormGroup;

  submitted = false;

  isSubmitting = false;
  errorMessage: string | null = null;

  constructor() {

    this.productForm = this.fb.group({

      name: ['', Validators.required],

      description: ['', Validators.required],

      price: [
        '',
        [
          Validators.required,
          Validators.min(1)
        ]
      ],

      stockQuantity: [
        '',
        [
          Validators.required,
          Validators.min(0)
        ]
      ]

    });

  }

  onRegister(): void {

    this.submitted = true;
    this.errorMessage = null;

    if (this.productForm.invalid) {

      this.productForm.markAllAsTouched();
      return;

    }

    this.isSubmitting = true;

    const payload: CreateProductPayload = this.productForm.value;

    this.productService.saveProduct(payload).subscribe({
      next: (product) => {
        this.isSubmitting = false;
        this.saveProduct.emit(product);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Unable to save product. Please try again.';
      }
    });

  }

}
