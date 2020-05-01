using System.Collections.Generic;

namespace FlourMill_1.Models
{
    public class TruckDriver
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; }

        public string BirthDate { get; set; }

        public long NationalId { get; set; }

        public string PhoneNumber { get; set; }

        public string JobNumber { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Administrator Administrator { get; set; }

        public int AdministratorID { get; set; }
    }
}