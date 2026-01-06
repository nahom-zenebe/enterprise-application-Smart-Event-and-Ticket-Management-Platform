using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Application.Payments.Interfaces;
using Stripe;

namespace Shared.Infrastructure.Payments.Gateways
{
    public class StripePaymentGateway : IPaymentGateway
    {
        public async Task<bool> ProcessAsync(decimal amount)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Stripe uses cents
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    }
                };

                var service = new PaymentIntentService();
                var intent = await service.CreateAsync(options);

                // In real systems:
                // intent.Status == "requires_confirmation" or "succeeded"

                return intent.Status == "succeeded" ||
                       intent.Status == "requires_confirmation";
            }
            catch (StripeException ex)
            {
                // Log ex.StripeError.Message
                return false;
            }
        }
    }
}
