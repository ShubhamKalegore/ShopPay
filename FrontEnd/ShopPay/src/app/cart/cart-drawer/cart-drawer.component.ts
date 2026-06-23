import { Component, EventEmitter, Inject, OnInit, PLATFORM_ID, Output } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { CartService } from '../../core/services/cart.service';
import { CartItem } from '../../core/models/cart-item';
import { AddressInfoComponent } from '../../auth/address-info/address-info.component';


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

  constructor(
    private cartService: CartService,
    @Inject(PLATFORM_ID) private platformId: object
  ) { }

  ngOnInit(): void {

    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items;
    });

    if (!this.isBrowser) {
      return;
    }

    const userData = localStorage.getItem('shopPayUser');

    if (userData) {

      const user = JSON.parse(userData);

      this.isAddressPresent = user.isAddressPresent;
      this.postalCode = user.postalCode;
    }
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
    const postalCode = savedAddress?.response?.postalCode
      ?? savedAddress?.response?.postal_code
      ?? savedAddress?.postalCode
      ?? null;

    if (user && postalCode) {
      const updatedUser = {
        ...user,
        isAddressPresent: true,
        postalCode
      };

      localStorage.setItem('shopPayUser', JSON.stringify(updatedUser));
      this.isAddressPresent = updatedUser.isAddressPresent;
      this.postalCode = updatedUser.postalCode;
    }

    this.closeAddressDialog();
  }

  private get isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }
}
