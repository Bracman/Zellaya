using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Board_Orders
    {
        [Key]
        public int id_order { get; set; }

        public DateTime date_order_creation { get; set; }

        public DateTime date_ready_order{ get; set; }

        public string order_number { get; set; }

        public string status_order {  get; set; }
        public int count_board {  get; set; }
    }
}
