using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Zellaya.Models
{
    public class Component
    {
        [Key]
        public int id_component { get; set; }

        [Required, StringLength(128)]
        public string name { get; set; } = default!;

        public string type_component { get; set; }

        public string part_number { get; set; }

        public string param_text { get; set; }

        public int is_active_component { get; set; }

        public List<StockMovement>? StockMovements { get; set; }

    }
}
