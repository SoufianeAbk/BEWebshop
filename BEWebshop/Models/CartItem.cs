using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEWebshop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Nullable - if null, item is in cart; if not null, item is part of an order
        public int? OrderId { get; set; }

        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual Order? Order { get; set; }

        [NotMapped]
        public decimal Subtotal => Quantity * Price;
    }
}