import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { CreatePaymentIntentRequest } from '../models/create-payment-intent-request';
import { CreatePaymentIntentResponse } from '../models/create-payment-intent-response';
import { VerifyPaymentResponse } from '../models/verify-payment-response';

@Injectable({
  providedIn: 'root'
})
export class StripeService {

  private readonly baseUrl =
    `${environment.apiUrl}/stripe`;

  constructor(
    private readonly http: HttpClient
  ) { }

  createPaymentIntent(
    request: CreatePaymentIntentRequest
  ): Observable<CreatePaymentIntentResponse> {

    return this.http.post<CreatePaymentIntentResponse>(
      `${this.baseUrl}/create-payment-intent`,
      request
    );

  }

  verifyPayment(
    paymentIntentId: string
  ): Observable<VerifyPaymentResponse> {

    return this.http.get<VerifyPaymentResponse>(
      `${this.baseUrl}/verify/${paymentIntentId}`
    );

  }

}