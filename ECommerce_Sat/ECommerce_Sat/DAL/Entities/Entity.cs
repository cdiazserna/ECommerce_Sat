using System.ComponentModel.DataAnnotations;

namespace ECommerce_Sat.DAL.Entities
{
    public class Entity
    {
        [Key]
        public virtual Guid Id { get; set; }

        public virtual string? CreatedDate { get; set; }

        public virtual string? ModifiedDate { get; set; }
    }
}
