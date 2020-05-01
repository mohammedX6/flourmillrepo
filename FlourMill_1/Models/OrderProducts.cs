namespace FlourMill_1.Models
{
    public class OrderProducts
    {
        public int id { get; set; }
        public string Badge { get; set; }

        public string pic { get; set; }

        public int price { get; set; }

        public int tons { get; set; }

        public Order order { get; set; }

        public int orderId { get; set; }
    }
}