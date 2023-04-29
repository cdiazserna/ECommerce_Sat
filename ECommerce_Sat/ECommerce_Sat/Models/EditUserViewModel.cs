using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.Models
{
    public class EditUserViewModel : Entity
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7048/images/noimage.png"
            : $"https://sales2023.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "País")]
        [NonEmptyGuid(ErrorMessage = "Debes de seleccionar un país.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Departmento/Estado")]
        [NonEmptyGuid(ErrorMessage = "Debes de seleccionar un estado.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "Ciudad")]
        [NonEmptyGuid(ErrorMessage = "Debes de seleccionar una ciudad.")]
        public Guid CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }
    }
}
