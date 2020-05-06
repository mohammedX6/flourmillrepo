using System.ComponentModel.DataAnnotations;

namespace DatingApp.Dtos
{
    public class TruckForRegisterDto
    {
        public string id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "You must specify password between 8 and 16 char")]
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
        public int AdministratorID { get; set; }
         
       
    }
}