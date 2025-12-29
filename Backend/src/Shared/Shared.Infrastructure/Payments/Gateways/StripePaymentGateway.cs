using System.Threading.Tasks;
using Shared.Application.Payments.Interfaces;

namespace Shared.Infrastructure.Payments.Gateways
{
    public class StripePaymentGateway : IPaymentGateway
    {
        public async Task<bool> ProcessAsync(decimal amount)
        {
            // Call Stripe SDK here
            await Task.Delay(300);
            return true;
        }
    }
}
