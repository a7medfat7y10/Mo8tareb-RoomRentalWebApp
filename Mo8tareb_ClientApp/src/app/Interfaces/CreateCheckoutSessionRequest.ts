



export interface CreateCheckoutSessionRequest {
  roomPrice: number;
  reservationId?: number;
  roomDescription?: string;
  roomTitle?: string;
  roomImages?: string[];
  successUrl: string;
  failureUrl: string;
}

