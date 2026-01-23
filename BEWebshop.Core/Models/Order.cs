using System.ComponentModel.DataAnnotations.Schema;

namespace BEWebshop.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        // ✅ Foreign key naar Identity User
        public string? UserId { get; set; }

        // ✅ Navigation property
        public virtual User? User { get; set; }

        // Andere order properties...
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
