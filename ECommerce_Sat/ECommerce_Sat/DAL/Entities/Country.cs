using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class Country : Entity
    {
        [Display(Name = "País")] //Nombre que quiero mostrar en la web
        [MaxLength(50)] //varchar(50)
        [Required(ErrorMessage = "El campo {0} es obligatorio")] //Not Null
        public string Name { get; set; }

        [Display(Name = "Estados")] //Nombre que quiero mostrar en la web
        public ICollection<State> States { get; set; }

        [Display(Name = "Número Estados")] //Nombre que quiero mostrar en la web
        public int StateNumber => States == null ? 0 : States.Count; //IF TERNARIO: SI state ES (==) null, ENTONCES (?) mandar un 0, SINO (:) mandar el COUNT

    }
}
