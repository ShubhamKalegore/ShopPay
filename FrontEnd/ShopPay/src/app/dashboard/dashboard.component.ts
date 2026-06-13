import { CommonModule } from '@angular/common';
import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  user: any = null;
  private readonly isBrowser: boolean;

  constructor(private router: Router, @Inject(PLATFORM_ID) platformId: object) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.user = this.getUser();
  }

  get displayName() {
    const fullName = [this.user?.firstName, this.user?.lastName].filter(Boolean).join(' ');
    return fullName || this.user?.email || 'ShopPay User';
  }

  logout() {
    if (this.isBrowser) {
      localStorage.removeItem('shopPayUser');
    }

    this.router.navigate(['/login']);
  }

  private getUser() {
    if (!this.isBrowser) {
      return null;
    }

    const storedUser = localStorage.getItem('shopPayUser');
    return storedUser ? JSON.parse(storedUser) : null;
  }
}
