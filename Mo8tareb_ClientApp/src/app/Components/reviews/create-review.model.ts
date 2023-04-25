export interface CreateReviewPayload {
    userId: string;
    roomId: number | undefined;
    comments: string;
    rating: number;
  }