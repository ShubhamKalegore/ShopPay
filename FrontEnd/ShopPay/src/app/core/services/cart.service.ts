import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CartItem } from '../models/cart-item';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private cartItems = new BehaviorSubject<CartItem[]>([]);
  cartItems$ = this.cartItems.asObservable();

  addToCart(product: Product): void {

    const items = this.cartItems.value;

    const existingItem = items.find(
      x => x.productId === product.productId
    );

    if (existingItem) {

      if (existingItem.quantity >= product.stockQuantity) {
        return;
      }

      existingItem.quantity++;

    } else {

      items.push({
        productId: product.productId,
        name: product.name,
        price: product.price,
        quantity: 1
      });

    }

    this.cartItems.next([...items]);
  }

  removeFromCart(productId: number): void {
    const items = this.cartItems.value.filter(
      x => x.productId !== productId
    );

    this.cartItems.next(items);
  }

  clearCart(): void {
    this.cartItems.next([]);
  }

  getCartItems(): CartItem[] {
    return this.cartItems.value;
  }

  getCartCount(): number {
    return this.cartItems.value.reduce(
      (total, item) => total + item.quantity,
      0
    );
  }

  getTotalAmount(): number {
    return this.cartItems.value.reduce(
      (total, item) => total + (item.price * item.quantity),
      0
    );
  }
}