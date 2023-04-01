using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class City : Entity
    {
        [Display(Name = "Ciudad")] //Nombre que quiero mostrar en la web
        [MaxLength(50)] //varchar(50)
        [Required(ErrorMessage = "El campo {0} es obligatorio")] //Not Null
        public string Name { get; set; }

        [Display(Name = "Estado")] //Nombre que quiero mostrar en la web
        public State State { get; set; }
    }
}
