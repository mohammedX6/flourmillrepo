namespace FlourMill_1.Models
{
    public class Wishlist
    {
        public int id { get; set; }

        public int price { get; set; }

        public string Badgename { get; set; }

        public string url { get; set; }

        public Bakery bakery { get; set; }

        public int BakeryId { get; set; }

        public Administrator Administrator { get; set; }

        public int AdministratorId { get; set; }

        public Product Product { get; set; }

        public int ProductId { get; set; }
    }
}