using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class State : Entity
    {
        [Display(Name = "Dpto/Estado")] //Nombre que quiero mostrar en la web
        [MaxLength(50)] //varchar(50)
        [Required(ErrorMessage = "El campo {0} es obligatorio")] //Not Null
        public string Name { get; set; }

        public Country Country { get; set; }
    }
}
