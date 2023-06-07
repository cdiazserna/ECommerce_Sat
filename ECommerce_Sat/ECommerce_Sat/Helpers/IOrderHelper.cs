using ECommerce_Sat.Common;
using ECommerce_Sat.Models;

namespace ECommerce_Sat.Helpers
{
    public interface IOrderHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel showCartViewModel);

        Task<Response> CancelOrderAsync(Guid orderId);
    }
}
