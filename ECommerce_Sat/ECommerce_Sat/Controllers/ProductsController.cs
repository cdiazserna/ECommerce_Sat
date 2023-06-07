using ECommerce_Sat.DAL;
using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Helpers;
using ECommerce_Sat.Models;
using ECommerce_Sat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace ECommerce_Sat.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IAzureBlobHelper _azureBlobHelper;
        private readonly IDropDownListHelper _dropDownListHelper;
        private readonly IFlashMessage _flashMessage;

        public ProductsController(DataBaseContext context, IAzureBlobHelper azureBlobHelper, IDropDownListHelper dropDownListHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _azureBlobHelper = azureBlobHelper;
            _dropDownListHelper = dropDownListHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AddProductViewModel addProductViewModel = new()
            {
                Categories = await _dropDownListHelper.GetDDLCategoriesAsync(),
            };

            return View(addProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductViewModel addProductViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;
                    if (addProductViewModel.ImageFile != null)
                        imageId = await _azureBlobHelper.UploadAzureBlobAsync(addProductViewModel.ImageFile, "products");

                    Product product = new()
                    {
                        Description = addProductViewModel.Description,
                        Name = addProductViewModel.Name,
                        Price = addProductViewModel.Price,
                        Stock = addProductViewModel.Stock,
                        CreatedDate = DateTime.Now,
                    };

                    product.ProductCategories = new List<ProductCategory>()
                    {
                        new ProductCategory
                        {
                            Category = await _context.Categories.FindAsync(addProductViewModel.CategoryId)
                        }
                    };

                    if (imageId != Guid.Empty)
                    {
                        product.ProductImages = new List<ProductImage>()
                        {
                            new ProductImage { 
                                ImageId = imageId,
                                CreatedDate = DateTime.Now,
                            }
                        };
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger(String.Format("Ya existe el producto {0}.", addProductViewModel.Name));
                        //ModelState.AddModelError(string.Empty, "Ya existe un producto con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            addProductViewModel.Categories = await _dropDownListHelper.GetDDLCategoriesAsync();
            return View(addProductViewModel);
        }

        public async Task<IActionResult> Edit(Guid? productId)
        {
            if (productId == null) return NotFound();

            Product product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            EditProductViewModel editProductViewModel = new()
            {
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };

            return View(editProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? Id, EditProductViewModel editProductViewModel)
        {
            if (Id != editProductViewModel.Id) return NotFound();

            try
            {
                Product product = await _context.Products.FindAsync(editProductViewModel.Id);

                //Aquí sobreescribo para luego guardar los cambios en BD
                product.Description = editProductViewModel.Description;
                product.Name = editProductViewModel.Name;
                product.Price = editProductViewModel.Price;
                product.Stock = editProductViewModel.Stock;
                product.ModifiedDate = DateTime.Now;

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con el mismo nombre.");
                else
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(editProductViewModel);
        }

        public async Task<IActionResult> Details(Guid? productId)
        {
            if (productId == null) return NotFound();

            Product product = await _context.Products
                .Include(p => p.ProductImages) // Inner Join entre Product - ProductImages
                .Include(p => p.ProductCategories) // Inner Join entre Product - ProductCategories
                .ThenInclude(pc => pc.Category) // Inner Join entre ProductCategories - Categories
                .FirstOrDefaultAsync(p => p.Id == productId);
            
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> AddImage(Guid? productId)
        {
            if (productId == null) return NotFound();

            Product product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            AddProductImageViewModel addProductImageViewModel = new()
            {
                ProductId = product.Id,
            };

            return View(addProductImageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel addProductImageViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync(addProductImageViewModel.ImageFile, "products");

                    Product product = await _context.Products.FindAsync(addProductImageViewModel.ProductId);
                
                    ProductImage productImage = new()
                    {
                        Product = product,
                        ImageId = imageId,
                        CreatedDate = DateTime.Now,
                    };

                    _context.Add(productImage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { productId = product.Id });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(addProductImageViewModel);
        }

        public async Task<IActionResult> DeleteImage(Guid? imageId)
        {
            if (imageId == null) return NotFound();

            ProductImage productImage = await _context.ProductImages
                .Include(pi => pi.Product)
                .FirstOrDefaultAsync(pi => pi.Id == imageId);

            if (productImage == null) return NotFound();

            await _azureBlobHelper.DeleteAzureBlobAsync(productImage.ImageId, "products");

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { productId = productImage.Product.Id });
        }

        public async Task<IActionResult> AddCategory(Guid? productId)
        {
            if (productId == null) return NotFound();

            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();

            List<Category> categories = product.ProductCategories.Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name, //Aquí coloco las N categoríes que le agregué a ese prod: GAMERS, TECHOLOGY
            }).ToList();

            AddProductCategoryViewModel addProductCategoryViewModel = new()
            {
                ProductId = product.Id,
                Categories = await _dropDownListHelper.GetDDLCategoriesAsync(categories),
            };

            return View(addProductCategoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddProductCategoryViewModel addProductCategoryViewModel)
        {
            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == addProductCategoryViewModel.ProductId);

            if (ModelState.IsValid)
            {
                try
                {
                    Category category = await _context.Categories.FindAsync(addProductCategoryViewModel.CategoryId);

                    if (product == null || category == null) return NotFound();

                    ProductCategory productCategory = new()
                    {
                        Product = product,
                        Category = category
                    };

                    _context.Add(productCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { productId = product.Id });
                }
                catch (Exception exception)
                {
                    addProductCategoryViewModel.Categories = await _dropDownListHelper.GetDDLCategoriesAsync();
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            List<Category> categories = product.ProductCategories.Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name, //Aquí coloco las N categoríes que le agregué a ese prod: GAMERS, TECHOLOGY
            }).ToList();

            addProductCategoryViewModel.Categories = await _dropDownListHelper.GetDDLCategoriesAsync(categories);
            return View(addProductCategoryViewModel);
        }

        public async Task<IActionResult> DeleteCategory(Guid? categoryId)
        {
            if (categoryId == null) return NotFound();

            ProductCategory productCategory = await _context.ProductCategories
                .Include(pc => pc.Product)
                .FirstOrDefaultAsync(pc => pc.Id == categoryId);
            if (productCategory == null) return NotFound();

            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { productId = productCategory.Product.Id });
        }

        public async Task<IActionResult> Delete(Guid? productId)
        {
            if (productId == null) return NotFound();

            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product productModel)
        {
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == productModel.Id);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            foreach (ProductImage productImage in product.ProductImages)
                await _azureBlobHelper.DeleteAzureBlobAsync(productImage.ImageId, "products");

            return RedirectToAction(nameof(Index));
        }
    }
}
