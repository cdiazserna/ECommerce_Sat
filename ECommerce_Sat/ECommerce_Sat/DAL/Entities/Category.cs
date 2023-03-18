using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class Category : Entity
    {
        [Display(Name = "Categoría")] //Nombre que quiero mostrar en la web
        [MaxLength(100)] //varchar(50)
        [Required(ErrorMessage = "El campo {0} es obligatorio")] //Not Null
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }
    }
}
