import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Order } from '../models/order.model';




@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private readonly baseUrl =
    `${environment.apiUrl}/orders`;

  constructor(
    private readonly http: HttpClient
  ) { }

  getOrders(): Observable<Order[]> {

    return this.http.get<Order[]>(
      this.baseUrl
    );

  }

  getOrder(
    orderId: number
  ): Observable<Order> {

    return this.http.get<Order>(
      `${this.baseUrl}/${orderId}`
    );

  }

  createOrder(
    order: Order
  ): Observable<Order> {

    return this.http.post<Order>(
      this.baseUrl,
      order
    );

  }

  updateOrder(
    orderId: number,
    order: Order
  ): Observable<Order> {

    return this.http.put<Order>(
      `${this.baseUrl}/${orderId}`,
      order
    );

  }

  deleteOrder(
    orderId: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.baseUrl}/${orderId}`
    );

  }

  getOrdersByPaymentStatus(
    isPaymentConfirmed: boolean
  ): Observable<Order[]> {

    return this.http.get<Order[]>(
      `${this.baseUrl}/payment-status/${isPaymentConfirmed}`
    );

  }

}