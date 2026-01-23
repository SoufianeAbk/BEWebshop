using System.ComponentModel.DataAnnotations;

namespace BEWebshop.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        // User relationship properties
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        // Navigation property
        public virtual ICollection<CartItem> OrderItems { get; set; } = new List<CartItem>();
    }
}