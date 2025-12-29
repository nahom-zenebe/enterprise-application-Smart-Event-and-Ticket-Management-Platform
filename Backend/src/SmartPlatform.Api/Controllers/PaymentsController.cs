using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Shared.Application.Payments.Commands.CreatePayment;
using Shared.Domain.Payments.Enums;

namespace Payments.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly CreatePaymentHandler _createPaymentHandler;

        public PaymentsController(CreatePaymentHandler createPaymentHandler)
        {
            _createPaymentHandler = createPaymentHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var command = new CreatePaymentCommand(
                request.UserId,
                request.Amount,
                request.Method
            );

            await _createPaymentHandler.Handle(command);

            return Ok(new
            {
                message = "Payment processed successfully"
            });
        }
    }
}
