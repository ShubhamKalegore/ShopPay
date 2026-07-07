import { Component, EventEmitter, Inject, OnInit, PLATFORM_ID, Output } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { CartService } from '../../core/services/cart.service';
import { CartItem } from '../../core/models/cart-item';
import { AddressInfoComponent } from '../../auth/address-info/address-info.component';
import { StripeService } from '../../core/services/stripe.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-cart-drawer',
  standalone: true,
  imports: [CommonModule, AddressInfoComponent],
  templateUrl: './cart-drawer.component.html',
  styleUrl: './cart-drawer.component.scss'
})
export class CartDrawerComponent implements OnInit {

  @Output() close = new EventEmitter<void>();
  isAddressDialogOpen = false;

  cartItems: CartItem[] = [];

  isAddressPresent = false;
  postalCode: string | null = null;

  userData = localStorage.getItem('shopPayUser');
  user = this.userData ? JSON.parse(this.userData) : null;

  constructor(
    private cartService: CartService,
    @Inject(PLATFORM_ID) private platformId: object,
    private readonly stripeService: StripeService,
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    this.cartService.cartItems$.subscribe(items => {
      debugger
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
                  totalAmount: this.subtotal,
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
}
