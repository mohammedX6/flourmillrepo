using System.ComponentModel.DataAnnotations;

namespace FlourMill_1.Dtos
{
    public class FirebaseForLoginDTO
    {
        public string email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}