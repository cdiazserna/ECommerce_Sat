using ECommerce_Sat.DAL;
using ECommerce_Sat.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Sat.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DataBaseContext _context;

        public CountriesController(DataBaseContext context)
        {
            _context = context;
        }

        //GET --> SELECT * FROM.....
        //POST --> CREATE/ INSERT....
        //PUT --> UPDATE
        //DELETE --> DELETE
        //PATCH --> UPDATE

        // GET: Countries
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries.ToListAsync());
        }

        // GET: Countries/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FirstOrDefaultAsync(m => m.Id == id); //Select * From Countries Where Id = '3rf2f-t23gf2-gh234g-g243g'
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FirstOrDefaultAsync(m => m.Id == id); //Select * From Countries Where Id = '7a216d04-3048-4757-9b02-f72ded5180bf'

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'DataBaseContext.Countries'  is null.");
            }
            var country = await _context.Countries.FindAsync(id); //Select * From Countries Where Id = '7a216d04-3048-4757-9b02-f72ded5180bf'
            if (country != null)
            {
                _context.Countries.Remove(country); 
            }

            await _context.SaveChangesAsync(); //Delete From Counties where Id = '7a216d04-3048-4757-9b02-f72ded5180bf'
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(Guid id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
