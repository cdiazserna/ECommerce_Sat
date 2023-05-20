using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class OrderDetail : Entity
    {
        public Order Order { get; set; }

        public TemporalSale? TemporalSale { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public Product Product { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Product == null ? 0 : (decimal)Quantity * Product.Price;
    }
}
