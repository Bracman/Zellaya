using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Order
    {
        public int Id { get; set; }                       // автоинкремент MySQL

        [Required, StringLength(32)]
        public string OrderNumber { get; set; } = default!;

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? ReadyDate { get; set; }

        public string status_order {  get; set; }

        // плата и её количество
        public int BoardId { get; set; }
        public Board Board { get; set; } = default!;
        public int BoardQuantity { get; set; }

        // выбранные радиокомпоненты (OrderComponent — связь «многие-к-многим» с количеством)
        public List<OrderComponent> Components { get; set; } = new();
    }
}
