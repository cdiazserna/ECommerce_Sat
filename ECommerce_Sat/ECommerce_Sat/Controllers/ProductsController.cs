using ECommerce_Sat.DAL;
using ECommerce_Sat.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Sat.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IAzureBlobHelper _azureBlobHelper;
        private readonly IDropDownListHelper _dropDownListHelper;

        public ProductsController(DataBaseContext context, IAzureBlobHelper azureBlobHelper, IDropDownListHelper dropDownListHelper)
        {
            _context = context;
            _azureBlobHelper = azureBlobHelper;
            _dropDownListHelper = dropDownListHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync());
        }
    }
}
