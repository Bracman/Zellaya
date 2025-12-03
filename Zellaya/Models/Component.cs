using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Component
    {
        [Key]
        public int id_component { get; set; }

        [Required, StringLength(128)]
        public string name { get; set; } = default!;

        // опционально: остаток на складе
        public int? StockQty { get; set; }
    }
}
