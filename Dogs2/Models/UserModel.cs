using System.ComponentModel.DataAnnotations;

namespace Dogs2.Models
{
    public class UsersModel
    {
        [Key]
        public int userId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
      
        [Display(Name = "Name")]
        public string displayName { get; set; }
        public string phone { get; set; }
    }
}
