using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

        [NotMapped]
        public string Room { get; set; }
    }
}
