using ECommerce_Sat.DAL;
using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace ECommerce_Sat.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IFlashMessage _flashMessage;

        public OrdersController(DataBaseContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders
                .Include(s => s.User)
                .Include(s => s.OrderDetails)
                .ThenInclude(sd => sd.Product)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? orderId)
        {
            if (orderId == null) return NotFound();

            Order order = await _context.Orders
                .Include(s => s.User)
                .Include(s => s.OrderDetails)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == orderId);

            if (order == null) return NotFound();

            return View(order);
        }

        public async Task<IActionResult> DispatchOrder(Guid? orderId)
        {
            if (orderId == null) return NotFound();

            Order order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            if (order.OrderStatus != OrderStatus.Nuevo)
                _flashMessage.Danger(String.Format("Solo se pueden enviar pedidos que estén en estado '{0}'.", OrderStatus.Nuevo));

            else
            {
                order.OrderStatus = OrderStatus.Despachado;
                order.ModifiedDate = DateTime.Now;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation(String.Format("El estado del pedido ha sido cambiado a '{0}'.", OrderStatus.Despachado));

            }

            return RedirectToAction(nameof(Details), new { orderId = order.Id });
        }

    }
}
