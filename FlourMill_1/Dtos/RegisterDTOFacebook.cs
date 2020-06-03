using System.ComponentModel.DataAnnotations;

namespace FlourMill_1.Dtos
{
    public class RegisterDTOFacebook
    {
        public string id { get; set; }

        [Required]
        public string Username { get; set; }

        public string Password { get; set; }

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