import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Product } from '../../core/models/product';
import { ProductService } from '../../core/services/product.service';
import { CartService } from '../../core/services/cart.service';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {

  products: Product[] = [];

  constructor(
    private productService: ProductService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  addToCart(product: Product): void {
    this.cartService.addToCart(product);
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (response) => {
        this.products = response;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  isOutOfStock(product: Product): boolean {

    const cartItem =
      this.cartService
        .getCartItems()
        .find(x => x.productId === product.productId);

    return !!cartItem &&
      cartItem.quantity >= product.stockQuantity;
  }
}