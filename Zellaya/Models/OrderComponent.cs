using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class OrderComponent
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public int ComponentId { get; set; }
        public Component Component { get; set; } = default!;

        [Range(1, 1_000_000)]
        public int Quantity { get; set; }
    }
}
