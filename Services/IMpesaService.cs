using Auction_System.Models;
using System.Threading.Tasks;

namespace Auction_System.Services
{
	public interface IMpesaService
	{
		Task<MpesaResponse> InitiateSTKPushAsync(
			string phoneNumber,
			decimal amount,
			int itemId,
			string userId);
	}
}