﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class TemporalSale : Entity
    {
        public User User { get; set; }

        public Product Product { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
    }
}
