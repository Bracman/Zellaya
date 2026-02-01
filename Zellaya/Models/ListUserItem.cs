namespace Zellaya.Models
{
    public class ListUserItem
    {
        public int id_user { get; set; }
        public string login { get; set; }        
        public string role_name { get; set; }
        public bool is_active_user { get; set; }
    }
}
