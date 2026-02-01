using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class OrderDetails
    {
        public string order_number {  get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ReadyDate { get; set; } 
        
        public string status_order {  get; set; }
        public List<OrderItemDetails> Items { get; set; }
    } 
}
