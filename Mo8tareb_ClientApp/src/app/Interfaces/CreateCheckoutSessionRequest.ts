



export interface CreateCheckoutSessionRequest {
  roomPrice: number;
  reservationId?: number;
  roomId?: number;
  roomDescription?: string;
  roomTitle?: string;
  roomImages?: string[];
  successUrl: string;
  failureUrl: string;
}

