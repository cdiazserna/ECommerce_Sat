using ECommerce_Sat.DAL;
using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Sat.Controllers
{
    public class CountriesController : Controller
    {
        #region Constructor
        private readonly DataBaseContext _context;

        public CountriesController(DataBaseContext context)
        {
            _context = context;
        }

        #endregion

        #region Countries Actions
        //GET --> SELECT * FROM.....
        //POST --> CREATE/ INSERT....
        //PUT --> UPDATE
        //DELETE --> DELETE
        //PATCH --> UPDATE

        // GET: Countries
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries
                .Include(c => c.States) //El Include me hace las veces del INNER JOIN
                .ToListAsync()); 
        }

        // GET: Countries/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Countries == null) return NotFound();

            var country = await _context.Countries
                .Include(c => c.States) //El Include me hace las veces del INNER JOIN
                .ThenInclude(s => s.Cities)
                .FirstOrDefaultAsync(m => m.Id == id); //Select * From Countries Where Id = '3rf2f-t23gf2-gh234g-g243g'
            
            if (country == null) return NotFound();

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
                    country.CreatedDate = DateTime.Now;
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
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
            if (id == null || _context.Countries == null) return NotFound();

            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            return View(country);
        }

        // POST: Countries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Country country)
        {
            if (id != country.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    country.ModifiedDate = DateTime.Now;
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
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
            if (id == null || _context.Countries == null) return NotFound();

            var country = await _context.Countries.FirstOrDefaultAsync(m => m.Id == id); //Select * From Countries Where Id = '7a216d04-3048-4757-9b02-f72ded5180bf'

            if (country == null) return NotFound();

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Countries == null)
                return Problem("Entity set 'DataBaseContext.Countries' is null.");

            var country = await _context.Countries.FindAsync(id); //Select * From Countries Where Id = '7a216d04-3048-4757-9b02-f72ded5180bf'
            if (country != null) _context.Countries.Remove(country); 

            await _context.SaveChangesAsync(); //Delete From Counties where Id = '7a216d04-3048-4757-9b02-f72ded5180bf'
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region States Actions

        [HttpGet]
        public async Task<IActionResult> AddState(Guid? countryId)
        {
            if (countryId == null) return NotFound();

            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == countryId);

            if (country == null) return NotFound();

            StateViewModel stateViewModel = new()
            {
                CountryId = country.Id,
            };

            return View(stateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel stateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Cities = new List<City>(),
                        Country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == stateViewModel.CountryId),
                        Name = stateViewModel.Name,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = null,
                    };

                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = stateViewModel.CountryId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un Dpto/Estado con el mismo nombre en este país.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(stateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditState(Guid? stateId)
        {
            if (stateId == null || _context.States == null) return NotFound();

            State state = await _context.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == stateId);

            if (state == null) return NotFound();

            StateViewModel stateViewModel = new()
            {
                CountryId = state.Country.Id,
                Id = state.Id,
                Name = state.Name,
                CreatedDate = state.CreatedDate,
            };

            return View(stateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(Guid countryId, StateViewModel stateViewModel)
        {
            if (countryId != stateViewModel.CountryId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Id = stateViewModel.Id,
                        Name = stateViewModel.Name,
                        CreatedDate = stateViewModel.CreatedDate,
                        ModifiedDate = DateTime.Now,
                    };

                    _context.Update(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = stateViewModel.CountryId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un estado con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(stateViewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> DetailsState(Guid? stateId)
        {
            if (stateId == null || _context.States == null) return NotFound();

            var state = await _context.States
                .Include(c => c.Country) //El Include me hace las veces del INNER JOIN
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(m => m.Id == stateId); //Select * From States Where Id = '3rf2f-t23gf2-gh234g-g243g'
            
            if (state == null) return NotFound();

            return View(state);
        }

        public async Task<IActionResult> DeleteState(Guid? stateId)
        {
            if (stateId == null || _context.States == null) return NotFound();

            var state = await _context.States
                .Include(c => c.Country)
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(m => m.Id == stateId);

            if (state == null) return NotFound();

            return View(state);
        }

        [HttpPost, ActionName("DeleteState")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStateConfirmed(Guid stateId)
        {
            if (_context.States == null) return Problem("Entity set 'DataBaseContext.States' is null.");

            var state = await _context.States
                .Include(c => c.Country)
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(m => m.Id == stateId);

            if (state != null) _context.States.Remove(state);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = state.Country.Id });
        }

        #endregion

        #region Cities Actions

        [HttpGet]
        public async Task<IActionResult> AddCity(Guid? stateId)
        {
            if (stateId == null) return NotFound();

            State state = await _context.States.FirstOrDefaultAsync(c => c.Id == stateId);

            if (state == null) return NotFound();

            CityViewModel cityViewModel = new()
            {
                StateId = state.Id,
            };

            return View(cityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(CityViewModel cityViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        State = await _context.States.FirstOrDefaultAsync(c => c.Id == cityViewModel.StateId),
                        Name = cityViewModel.Name, //Avellaneda
                        CreatedDate = DateTime.Now,
                        ModifiedDate = null,
                    };

                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsState), new { stateId = cityViewModel.StateId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe una ciudad con el mismo nombre en este dpto/estado.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(cityViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditCity(Guid? cityId)
        {
            if (cityId == null || _context.Cities == null) return NotFound();

            City city = await _context.Cities
                .Include(s => s.State)
                .FirstOrDefaultAsync(s => s.Id == cityId);

            if (city == null) return NotFound();

            CityViewModel cityViewModel = new()
            {
                StateId = city.State.Id,
                Id = city.Id,
                Name = city.Name,
                CreatedDate = city.CreatedDate,
            };

            return View(cityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(Guid stateId, CityViewModel cityViewModel)
        {
            if (stateId != cityViewModel.StateId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        Id = cityViewModel.Id,
                        Name = cityViewModel.Name,
                        CreatedDate = cityViewModel.CreatedDate,
                        ModifiedDate = DateTime.Now,
                    };

                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsState), new { stateId = cityViewModel.StateId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe una ciudad con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(cityViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsCity(Guid? cityId)
        {
            if (cityId == null || _context.Cities == null) return NotFound();

            var city = await _context.Cities
                .Include(c => c.State) //El Include me hace las veces del INNER JOIN
                .FirstOrDefaultAsync(m => m.Id == cityId); //Select * From States Where Id = '3rf2f-t23gf2-gh234g-g243g'

            if (city == null) return NotFound();

            return View(city);
        }

        public async Task<IActionResult> DeleteCity(Guid? cityId)
        {
            if (cityId == null || _context.Cities == null) return NotFound();

            City city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == cityId);

            if (city == null) return NotFound();

            return View(city);
        }

        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCityConfirmed(Guid cityId)
        {
            if (_context.States == null) return Problem("Entity set 'DataBaseContext.Cities' is null.");

            City city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == cityId);

            if (city != null) _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(DetailsState), new { stateId = city.State.Id });
        }

        #endregion
    }
}
