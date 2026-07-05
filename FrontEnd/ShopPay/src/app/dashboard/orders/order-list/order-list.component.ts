import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-list.component.html'
})
export class OrderListComponent implements OnInit {

  constructor (
    private orderService : OrderService
  ) {

  }
  ngOnInit(): void {
    this.orderService.getOrders().subscribe((orders: any[]) => {
      this.pendingOrders = orders.filter(
        order => order.orderStatus?.toUpperCase() === 'PENDING'
      );

      this.confirmedOrders = orders.filter(
        order => order.orderStatus?.toUpperCase() === 'CONFIRMED'
      );
    });
  }

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