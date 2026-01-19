using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class StockMovement
    {
        [Key]
        public int id_movement { get; set; }
        public int id_component { get; set; }
        public int qty { get; set; }
        public string movement_type { get; set; } 
        public string? comment { get; set; }
        public DateTime created_at { get; set; }

        public Component Component { get; set; }
    }
}
