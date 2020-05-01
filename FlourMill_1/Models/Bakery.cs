using System.Collections.Generic;

namespace FlourMill_1.Models
{
    public class Bakery
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; }

        public string BirthDate { get; set; }

        public long NationalId { get; set; }

        public string PhoneNumber { get; set; }

        public string JobNumber { get; set; }

        public string address { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }

        public ICollection<Order> Order { get; set; }
        public ICollection<Wishlist> wishlists { get; set; }

        public ICollection<ProductRate> ProductRates { get; set; }
    }
}