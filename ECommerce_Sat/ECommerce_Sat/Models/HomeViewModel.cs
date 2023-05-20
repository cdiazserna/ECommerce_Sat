using ECommerce_Sat.DAL.Entities;

namespace ECommerce_Sat.Models
{
    public class HomeViewModel
    {
        public ICollection<Product> Products { get; set; }

        //Esta propiedad me muestra cuánto productos llevo agregados al carrito de compras.
        public float Quantity { get; set; }
    }
}
