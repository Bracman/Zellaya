using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;

namespace Zellaya.Models
{
    public class Roles
    {
        [Key]
        public int id_role {  get; set; }

        public string role_name { get; set; }

        public string role_description { get; set; }
    }
}
