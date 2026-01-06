using System.Threading.Tasks;

namespace Shared.Application.Payments.Interfaces
{
    public interface IPaymentGateway
    {
        Task<bool> ProcessAsync(decimal amount);
    }
}

