using ECommerce_Sat.DAL.Entities;

namespace ECommerce_Sat.Models
{
    public class CityViewModel : City
    {
        public Guid StateId { get; set; }
    }
}
