import { Component, EventEmitter, Inject, OnInit, PLATFORM_ID, Output } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { CartService } from '../../core/services/cart.service';
import { CartItem } from '../../core/models/cart-item';
import { AddressInfoComponent } from '../../auth/address-info/address-info.component';
import { StripeService } from '../../core/services/stripe.service';
import { Router } from '@angular/router';
import confetti from 'canvas-confetti';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart-drawer',
  standalone: true,
  imports: [CommonModule, AddressInfoComponent, FormsModule],
  templateUrl: './cart-drawer.component.html',
  styleUrl: './cart-drawer.component.scss'
})
export class CartDrawerComponent implements OnInit {

  // | Coupon  | Discount |
  // | ------- | -------: |
  // | SAVE10  |     ₹100 |
  // | WELCOME |     ₹200 |
  // | SHOPPAY |     ₹500 |

  @Output() close = new EventEmitter<void>();
  isAddressDialogOpen = false;

  cartItems: CartItem[] = [];

  isAddressPresent = false;
  postalCode: string | null = null;

  userData = localStorage.getItem('shopPayUser');
  user = this.userData ? JSON.parse(this.userData) : null;

  couponCode = '';
  appliedCoupon = '';
  discount = 0;
  isCouponExpanded = false;

  constructor(
    private cartService: CartService,
    @Inject(PLATFORM_ID) private platformId: object,
    private readonly stripeService: StripeService,
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items;
    });

    if (!this.isBrowser) {
      return;
    }

    this.loadUserAddressState();
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

  openAddressDialog(): void {
    this.isAddressDialogOpen = true;
  }

  closeAddressDialog(): void {
    this.isAddressDialogOpen = false;
  }

  saveAddress(savedAddress: any): void {
    if (!this.isBrowser) {
      this.closeAddressDialog();
      return;
    }

    const userData = localStorage.getItem('shopPayUser');
    const user = userData ? JSON.parse(userData) : null;
    const updatedUserFromAddress = savedAddress?.updatedUser;
    const postalCode = savedAddress?.response?.postalCode
      ?? savedAddress?.response?.postal_code
      ?? savedAddress?.postalCode
      ?? updatedUserFromAddress?.postalCode
      ?? null;

    if (updatedUserFromAddress) {
      this.setUserAddressState(updatedUserFromAddress);
    } else if (user && postalCode) {
      const updatedUser = {
        ...user,
        isAddressPresent: true,
        postalCode
      };

      localStorage.setItem('shopPayUser', JSON.stringify(updatedUser));
      this.setUserAddressState(updatedUser);
    }

    this.closeAddressDialog();
  }

  private loadUserAddressState(): void {
    const userData = localStorage.getItem('shopPayUser');

    if (!userData) {
      return;
    }

    this.setUserAddressState(JSON.parse(userData));
  }

  private setUserAddressState(user: any): void {
    const postalCode = user?.postalCode
      ?? user?.postal_code
      ?? user?.PostalCode
      ?? null;

    this.postalCode = postalCode;
    this.isAddressPresent = Boolean(user?.isAddressPresent || user?.IsAddressPresent || postalCode);
  }

  private get isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  checkout(): void {

    if (this.cartItems.length === 0) {
      return;
    }

    if (!this.postalCode) {
      this.openAddressDialog();
      return;
    }

    const request = {
      items: this.cartItems.map(item => ({
        productId: item.productId,
        productName: item.name,
        unitPrice: item.price,
        quantity: item.quantity,
        currency: 'inr'
      }))
    };

    this.stripeService
      .createPaymentIntent(request)
      .subscribe({

        next: response => {

          this.router.navigate(
            ['/payment'],
            {
              state: {
                clientSecret: response.clientSecret,
                paymentIntentId: response.paymentIntentId,
                order: {
                  userId: this.user.userId,
                  totalAmount: this.subtotal - this.discount,
                  orderStatus: 'PENDING',
                  orderItems: this.cartItems.map(item => ({
                    productId: item.productId,
                    quantity: item.quantity,
                    unitPrice: item.price
                  }))
                },
                cartItems: this.cartItems
              }
            }
          );

        },

        error: error => {

          console.error(error);

        }

      });

  }

  applyCoupon(): void {
    // if (this.appliedCoupon) {
    //   alert('Coupon has already been applied.');
    //   return;
    // }
    const code = this.couponCode.trim().toUpperCase();

    switch (code) {

      case 'SAVE10':

        this.appliedCoupon = code;
        this.discount = 100;

        this.launchConfetti();

        break;

      case 'WELCOME':

        this.appliedCoupon = code;
        this.discount = 200;

        this.launchConfetti();

        break;

      case 'SHOPPAY':

        this.appliedCoupon = code;
        this.discount = 500;

        this.launchConfetti();

        break;

      default:

        alert('Invalid coupon code.');

        this.appliedCoupon = '';
        this.discount = 0;

        break;

    }

  }

  private launchConfetti(): void {

    confetti({
      particleCount: 180,
      spread: 90,
      origin: { y: 0.6 }
    });

  }

  toggleCouponSection(): void {
    this.isCouponExpanded = !this.isCouponExpanded;
  }
}
