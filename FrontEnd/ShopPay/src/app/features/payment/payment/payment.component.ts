import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import {
  loadStripe,
  Stripe,
  StripeElements
} from '@stripe/stripe-js';
import { environment } from '../../../../environments/environment';
import { Order } from '../../../core/models/order.model';
import { OrderService } from '../../../core/services/order.service';
import { StripeService } from '../../../core/services/stripe.service';


@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {

  clientSecret!: string;

  order!: Order;

  paymentIntentId!: string;

  stripe: Stripe | null = null;

  elements: StripeElements | null = null;

  constructor(
    private readonly router: Router,
    private readonly orderService: OrderService,
    private readonly stripeService: StripeService
  ) { }

  async ngOnInit(): Promise<void> {

    const navigation =
      this.router.getCurrentNavigation();

    const state =
      navigation?.extras.state ?? this.getBrowserHistoryState();

    this.clientSecret =
      state?.['clientSecret'];

    this.paymentIntentId =
      state?.['paymentIntentId'];

    this.order =
      state?.['order'];

    if (!this.clientSecret || !this.paymentIntentId) {
      this.router.navigate(['/dashboard']);
      return;
    }

    await this.initializeStripe();

  }

  private getBrowserHistoryState(): Record<string, unknown> | undefined {

    if (typeof history === 'undefined') {
      return undefined;
    }

    return history.state;

  }

  private async initializeStripe(): Promise<void> {

    this.stripe =
      await loadStripe(environment.stripePublishableKey);

    if (!this.stripe) {
      console.error('Failed to initialize Stripe.');
      return;
    }

    this.elements =
      this.stripe.elements({
        clientSecret: this.clientSecret
      });

    const paymentElement =
      this.elements.create('payment');

    paymentElement.mount('#card-element');

  }

  async pay(): Promise<void> {

    if (!this.stripe || !this.elements) {
      return;
    }

    const result = await this.stripe.confirmPayment({

      elements: this.elements,

      confirmParams: {

        return_url: 'http://localhost:4200/payment/success'

      },

      redirect: 'if_required'

    });

    if (result.error) {

      console.error(result.error.message);

      await this.router.navigate(['/payment/failed']);

      return;

    }

    if (result.paymentIntent?.status === 'succeeded') {

      console.log(result.paymentIntent);

      await this.verifyPayment(
        result.paymentIntent.id
      );

    }
    else {

      await this.router.navigate(
        ['/payment/failed'],
        {
          state: {
            paymentIntentId: result.paymentIntent?.id,
            status: result.paymentIntent?.status
          }
        });

    }

  }


  private async verifyPayment(paymentIntentId: string): Promise<void> {

    this.stripeService
      .verifyPayment(paymentIntentId)
      .subscribe({

        next: response => {

          console.log(response);

          if (response.isPaid) {

            this.createOrder(paymentIntentId);

          }
          else {

            this.router.navigate(['/payment/failed']);

          }

        },

        error: error => {

          console.error(error);

          this.router.navigate(['/payment/failed']);

        }

      });

  }


  private createOrder(paymentIntentId: string): void {

    this.order.stripePaymentIntentId =
      paymentIntentId;

    this.order.isPaymentConfirmed =
      false;

    this.order.orderStatus = "CONFIRMED"

    this.orderService
      .createOrder(this.order)
      .subscribe({

        next: response => {

          console.log('Order Created', response);

          this.router.navigate(
            ['/payment/success'],
            {
              state: {
                order: response
              }
            });

        },

        error: error => {

          console.error(error);

          this.router.navigate(
            ['/payment/failed']
          );

        }

      });

  }

}
