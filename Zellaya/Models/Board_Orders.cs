using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Board_Orders
    {
        [Key]
        public int id_order { get; set; }

        public DateTime date_order_creation { get; set; }
    }
}
