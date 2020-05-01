using System.ComponentModel.DataAnnotations;

namespace FlourMill_1.Dtos
{
    public class BakeryRegisterDTO

    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 char")]
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

        [Required]
        public string address { get; set; }

        [Required]
        public double latitude { get; set; }

        [Required]
        public double longitude { get; set; }
    }
}