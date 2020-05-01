namespace FlourMill_1.Models
{
    public class ProductRate
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string RateDate { get; set; }

        public string RateText { get; set; }

        public Product Product { get; set; }

        public int ProductID { get; set; }

        public Administrator Administrator { get; set; }

        public int AdministratorID { get; set; }

        public Bakery Bakery { get; set; }

        public int BakeryId { get; set; }
    }
}