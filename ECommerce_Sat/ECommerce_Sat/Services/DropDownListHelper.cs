using ECommerce_Sat.DAL;
using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ECommerce_Sat.Services
{
    public class DropDownListHelper : IDropDownListHelper
    {
        public readonly DataBaseContext _context;

        public DropDownListHelper(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync()
        {
            List<SelectListItem> listCategories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCategories.Insert(0, new SelectListItem
            {
                Text = "Selecione una categoría...",
                Value = "0",
            });

            return listCategories;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLCountriesAsync()
        {
            List<SelectListItem> listCountries = await _context.Countries
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCountries.Insert(0, new SelectListItem
            {
                Text = "Selecione un país...",
                Value = "0",
            });

            return listCountries;
        }


        public async Task<IEnumerable<SelectListItem>> GetDDLStatesAsync(Guid countryId)
        {
            List<SelectListItem> listStates = await _context.States
                .Where(s => s.Country.Id == countryId)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
                .OrderBy(s => s.Text)
                .ToListAsync();

            listStates.Insert(0, new SelectListItem
            {
                Text = "Selecione un estado...",
                Value = "0",
            });

            return listStates;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLCitiesAsync(Guid stateId)
        {
            List<SelectListItem> listCities = await _context.Cities
                .Where(c => c.State.Id == stateId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCities.Insert(0, new SelectListItem
            {
                Text = "Selecione una ciudad...",
                Value = "0",
            });

            return listCities;
        }
    }
}
