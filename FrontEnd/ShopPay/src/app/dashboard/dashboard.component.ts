import { CommonModule } from '@angular/common';
import { Component, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { CartService } from '../core/services/cart.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy{
  user: any = null;
  private readonly isBrowser: boolean;
  cartCount = 0;
  private cartSubscription?: Subscription;

  constructor(
    private router: Router,
    @Inject(PLATFORM_ID) platformId: object,
    private authService: AuthService,
    private cartService: CartService
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.user = this.getUser();
  }

  ngOnInit(): void {
    this.cartSubscription =
      this.cartService.cartItems$.subscribe(items => {
        this.cartCount = items.reduce(
          (total, item) => total + item.quantity,
          0
        );
      });
  }

  ngOnDestroy(): void {
    this.cartSubscription?.unsubscribe();
  }

  get displayName() {
    const fullName = [this.user?.firstName, this.user?.lastName].filter(Boolean).join(' ');
    return fullName || this.user?.email || 'ShopPay User';
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        localStorage.removeItem('shopPayUser');
        this.router.navigate(['/login']);
      },
      error: () => {
        localStorage.removeItem('shopPayUser');
        this.router.navigate(['/login']);
      }
    });
  }

  private getUser() {
    if (!this.isBrowser) {
      return null;
    }

    const storedUser = localStorage.getItem('shopPayUser');
    return storedUser ? JSON.parse(storedUser) : null;
  }
}
