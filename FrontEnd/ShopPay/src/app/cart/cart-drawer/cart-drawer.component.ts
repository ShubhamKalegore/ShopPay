import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../core/services/cart.service';
import { CartItem } from '../../core/models/cart-item';


@Component({
  selector: 'app-cart-drawer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart-drawer.component.html',
  styleUrl: './cart-drawer.component.scss'
})
export class CartDrawerComponent implements OnInit {
  @Output() close = new EventEmitter<void>();
  cartItems: CartItem[] = [];

  constructor(
    private cartService: CartService
  ) { }

  ngOnInit(): void {
    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items;
    });
  }

  increaseQuantity(product: CartItem): void {
    if (product.quantity >= product.stockQuantity) {
      return;
    }
    this.cartService.increaseQuantity(product.productId);
  }

  decreaseQuantity(productId: number): void {
    this.cartService.decreaseQuantity(productId);
  }

  removeItem(productId: number): void {
    this.cartService.removeFromCart(productId);
  }
  closeDrawer(): void {
    this.close.emit();
  }

  get subtotal(): number {
    return this.cartService.getTotalAmount();
  }
}