import { CommonModule } from '@angular/common';
import { Component, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { CartService } from '../core/services/cart.service';
import { Subscription } from 'rxjs';
import { CartDrawerComponent } from '../cart/cart-drawer/cart-drawer.component';
import { AiService } from '../core/services/ai.service';
import { ChatMessage } from '../core/models/chat-message';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet, CartDrawerComponent,
        FormsModule,
        RouterLink,
        RouterLinkActive,
        RouterOutlet,
        CartDrawerComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy{
  user: any = null;
  private readonly isBrowser: boolean;
  cartCount = 0;
  private cartSubscription?: Subscription;
  isCartOpen = false;
  isProfileOpen = false;
  isAiChatOpen = false;

  messages: ChatMessage[] = [
    {
      sender: 'ai',
      message: "👋 Hi! I'm your ShopPay AI Assistant.",
      timestamp: new Date()
    }
  ];

  currentMessage = '';

  isSending = false;

  constructor(
    private router: Router,
    @Inject(PLATFORM_ID) platformId: object,
    private authService: AuthService,
    private cartService: CartService,
    private aiService: AiService
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


  toggleAiChat(): void {
    this.isAiChatOpen = !this.isAiChatOpen;
  }

  closeAiChat(): void {
    this.isAiChatOpen = false;
  }

  get displayName() {
    return this.profileName || this.profileEmail || 'ShopPay User';
  }

  get profileUserId() {
    return this.user?.userId ?? this.user?.UserId ?? 'N/A';
  }

  get profileName() {
    const backendName = this.user?.userName ?? this.user?.UserName;
    const fullName = [this.user?.firstName, this.user?.lastName].filter(Boolean).join(' ');
    return backendName || fullName || 'ShopPay User';
  }

  get profileEmail() {
    return this.user?.userEmail ?? this.user?.UserEmail ?? this.user?.email ?? this.user?.Email ?? 'N/A';
  }

  get profileInitial() {
    return this.profileName.charAt(0).toUpperCase();
  }

  toggleProfile(): void {
    this.isProfileOpen = !this.isProfileOpen;
  }

  logout() {
    this.isProfileOpen = false;
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

  toggleCart(): void {
    this.isCartOpen = !this.isCartOpen;
  }

  closeCart(): void {
    this.isCartOpen = false;
  }

  sendMessage(): void {
debugger
    const message = this.currentMessage.trim();

    if (!message || this.isSending) {
      return;
    }

    this.messages.push({
      sender: 'user',
      message,
      timestamp: new Date()
    });

    this.currentMessage = '';

    this.isSending = true;

    this.aiService.sendMessage(message)
      .subscribe({
        next: (response) => {

          this.messages.push({
            sender: 'ai',
            message: response.message,
            timestamp: new Date()
          });

          this.isSending = false;
        },
        error: () => {

          this.messages.push({
            sender: 'ai',
            message: 'Something went wrong. Please try again.',
            timestamp: new Date()
          });

          this.isSending = false;
        }
      });
  }
}
