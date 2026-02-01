using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Orders
    {
        [Key]
       public int id_order { get; set; }
       public string num_order { get; set; } = default!;
       public DateTime? CreatedDate { get; set; }
       public DateTime? ReadyDate { get; set; }
       
      public string status_order { get; set; }
       public int count_board {  get; set; }

    }
}
