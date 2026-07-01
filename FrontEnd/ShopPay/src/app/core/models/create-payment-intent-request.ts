import { PaymentItem } from './payment-item';

export interface CreatePaymentIntentRequest {
  items: PaymentItem[];
}