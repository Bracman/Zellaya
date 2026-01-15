using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class OrderItemDetails
    {
        public string name_component { get; set; }       
        public string type_component { get; set; }
        public string part_number { get; set; }
        public int count_component { get; set; }
    }

}
