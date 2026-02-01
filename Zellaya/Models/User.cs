using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class User
    {
        [Key] 
        public int id_user { get; set; }

        public int id_role { get; set; }
        public string login { get; set; }
        public string password_hash { get; set; }

        public int is_active_user { get; set; }
        
    }
}