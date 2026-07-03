import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-list.component.html'
})
export class OrderListComponent {

  selectedTab: 'confirmed' | 'pending' = 'confirmed';

  confirmedOrders: any[] = [];

  pendingOrders: any[] = [];

  showConfirmed(): void {
    this.selectedTab = 'confirmed';
  }

  showPending(): void {
    this.selectedTab = 'pending';
  }

}