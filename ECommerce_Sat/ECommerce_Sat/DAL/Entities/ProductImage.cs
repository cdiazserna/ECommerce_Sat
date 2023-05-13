using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class ProductImage : Entity
    {
        public Product Product { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7158/images/NoImage.png"
            : $"https://sales2023.blob.core.windows.net/products/{ImageId}";
    }
}
