using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class InventoryBalanceRow
    {
        public int id_component { get; set; }
        public string name { get; set; }
        public string type_component { get; set; }
        public string part_number { get; set; }
        public int stock_qty { get; set; }
    }


}
