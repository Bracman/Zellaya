using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Documents
    {
        [Key]
        public int id_document { get; set; }
        public string file_name { get; set; }

        public string file_ext { get; set; }

        public string file_path { get; set; }

        

    }


}
