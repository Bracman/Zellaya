using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Board
    {
        [Key]
        public int id_board { get; set; }
        public string name_board { get; set; } = default!;

        public string code_board { get; set; } = default!;

    }


}
