using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class User
    {
        [Key]  // Явно указываем, что это первичный ключ
        public int Id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
}