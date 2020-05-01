using System.ComponentModel.DataAnnotations;

namespace DatingApp.Dtos
{
    public class UserForRegisterDto
    {
        public string id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "You must specify password between 6 and 16 chars")]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public long NationalId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string JobNumber { get; set; }

        public int TotalFlourMillPayment { get; set; }
    }
}