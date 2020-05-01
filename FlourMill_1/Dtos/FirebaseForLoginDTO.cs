using System.ComponentModel.DataAnnotations;

namespace DatingApp.Dtos
{
    public class FirebaseForLoginDTO
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}