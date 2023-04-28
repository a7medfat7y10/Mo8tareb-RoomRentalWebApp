using System.ComponentModel.DataAnnotations;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos
{
	public class CustomerPortalRequest
	{
		[Required]
		public string ReturnUrl { get; set; }
	}
}