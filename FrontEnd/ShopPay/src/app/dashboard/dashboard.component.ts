import { CommonModule } from '@angular/common';
import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  user: any = null;
  private readonly isBrowser: boolean;

  constructor(private router: Router, @Inject(PLATFORM_ID) platformId: object, private authService: AuthService) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.user = this.getUser();
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
