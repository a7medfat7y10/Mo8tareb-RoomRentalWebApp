using System.ComponentModel.DataAnnotations;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos
{
	public class CreateCheckoutSessionRequest
	{
		[Required]
		public long RoomPrice { get; set; }
		public int ReservationId { get; set; }
        public int RoomId { get; set; }

        public string? RoomDescription { get; set; }
		//public string? RoomTitle { get; set; }
		//public List<string>? RoomImages { get; set; } = new List<string>();

		[Required]
		public string? SuccessUrl { get; set; }
		[Required]
		public string? FailureUrl { get; set; }
	}
}