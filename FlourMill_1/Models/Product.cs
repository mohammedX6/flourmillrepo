using System.Collections.Generic;

namespace FlourMill_1.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string URL { get; set; }

        public string BadgeName { get; set; }

        public string BadgeType { get; set; }

        public string BadgeSize { get; set; }

        public string ProductionDate { get; set; }

        public string ExpireDate { get; set; }

        public string Usage { get; set; }

        public string ProductDescription { get; set; }

        public int price { get; set; }

        public Administrator Administrator { get; set; }

        public int AdministratorID { get; set; }

        public ICollection<ProductRate> ProductRate { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
    }
}