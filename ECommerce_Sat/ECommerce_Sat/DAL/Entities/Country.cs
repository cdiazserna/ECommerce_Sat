using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class Country : Entity
    {
        [Display(Name = "País")] //Nombre que quiero mostrar en la web
        [MaxLength(50)] //varchar(50)
        [Required(ErrorMessage = "El campo {0} es obligatorio")] //Not Null
        public string Name { get; set; }
        
        public ICollection<State> State { get; set; }

        public int StateNumber => State == null ? 0 : State.Count; //IF TERNARIO: SI state ES (==) null, ENTONCES (?) mandar un 0, SINO (:) mandar el COUNT


    }
}
