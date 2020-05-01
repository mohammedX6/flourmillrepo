using System.Collections.Generic;

namespace FlourMill_1.Models
{
    public class SuperVisor
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; }

        public string BirthDate { get; set; }

        public long NationalId { get; set; }

        public string JobNumber { get; set; }

        public ICollection<Report> Report { get; set; }
    }
}