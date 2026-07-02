import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import {
  loadStripe,
  Stripe,
  StripeElements
} from '@stripe/stripe-js';
import { environment } from '../../../../environments/environment';


@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {

  clientSecret!: string;

  paymentIntentId!: string;

  stripe: Stripe | null = null;

  elements: StripeElements | null = null;

  constructor(
    private readonly router: Router
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

      return;

    }

    if (result.paymentIntent?.status === 'succeeded') {

      console.log(result.paymentIntent);

      const navigated = await this.router.navigate(
        ['/payment/success'],
        {
          state: {
            paymentIntentId: result.paymentIntent.id
          }
        }
      );

      console.log('Navigation:', navigated);

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

}
