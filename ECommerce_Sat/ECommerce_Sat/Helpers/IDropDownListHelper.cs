using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce_Sat.Helpers
{
    public interface IDropDownListHelper
    {
        Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync(); //DDL = Drop Down List

        Task<IEnumerable<SelectListItem>> GetDDLCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetDDLStatesAsync(Guid countryId);

        Task<IEnumerable<SelectListItem>> GetDDLCitiesAsync(Guid stateId);
    }
}
