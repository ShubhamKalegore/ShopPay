import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
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
import { Product } from '../../core/models/product';

@Component({
  selector: 'app-product-info',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.scss'
})
export class ProductInfoComponent implements OnChanges {

  private readonly fb = inject(FormBuilder);
  private readonly productService = inject(ProductService);

  @Output()
  saveProduct = new EventEmitter<any>();

    @Input()
  product: Product | null = null;

  @Input()
  isEditMode = false;

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

  ngOnChanges(changes: SimpleChanges): void {

    if (this.product) {

      this.productForm.patchValue({
        name: this.product.name,
        description: this.product.description,
        price: this.product.price,
        stockQuantity: this.product.stockQuantity
      });

    } else {

      this.productForm.reset();

    }

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

    if (this.isEditMode && this.product) {

      this.productService
        .updateProduct(this.product.productId, payload)
        .subscribe({
          next: product => {
            this.isSubmitting = false;
            this.saveProduct.emit(product);
          },
          error: () => {
            this.isSubmitting = false;
            this.errorMessage = 'Unable to update product.';
          }
        });

    }
    else {

      this.productService
        .saveProduct(payload)
        .subscribe({
          next: product => {
            this.isSubmitting = false;
            this.saveProduct.emit(product);
          },
          error: () => {
            this.isSubmitting = false;
            this.errorMessage = 'Unable to save product.';
          }
        });

    }

  }

}
