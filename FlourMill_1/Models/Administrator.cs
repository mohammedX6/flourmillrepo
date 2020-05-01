using System.Collections.Generic;

namespace FlourMill_1.Models
{
    public class Administrator
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

        public double TotalFlourMillPayment { get; set; }

        public ICollection<Product> Product { get; set; }

        public ICollection<Order> Order { get; set; }

        public ICollection<Report> Report { get; set; }

        public ICollection<ProductRate> ProductRate { get; set; }

        public ICollection<AdminRate> AdminRate { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }

        public ICollection<TruckDriver> TruckDrivers { get; set; }
    }
}