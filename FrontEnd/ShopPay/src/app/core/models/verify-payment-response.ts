export interface VerifyPaymentResponse {
  paymentIntentId: string;
  paymentStatus: string;
  amountTotal: number;
  currency: string;
  isPaid: boolean;
}