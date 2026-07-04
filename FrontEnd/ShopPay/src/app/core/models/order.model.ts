export interface Order {

  orderId: number;

  userId: number;

  shippingAddressId?: number;

  billingAddressId?: number;

  totalAmount: number;

  orderStatus: string;

  isPaymentConfirmed: boolean;

  stripePaymentIntentId?: string;

}