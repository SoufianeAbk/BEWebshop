using System.ComponentModel.DataAnnotations.Schema;

namespace BEWebshop.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";

        // ✅ Foreign key naar Identity User
        public string? UserId { get; set; }

        // ✅ Navigation property
        public virtual User? User { get; set; }

        // ✅ Navigation property naar CartItems (niet OrderItems)
        public virtual ICollection<CartItem> OrderItems { get; set; } = new List<CartItem>();
    }
}