using System.Collections.Generic;

namespace FlourMill_1.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string Order_Date { get; set; }

        public double TotalTons { get; set; }

        public int OrderStatues { get; set; }

        public string CustomerName { get; set; }

        public string Destination { get; set; }
        public double TotalPayment { get; set; }

        public int ShipmentPrice { get; set; }

        public string OrderComment { get; set; }

        public Administrator Administrator { get; set; }

        public int AdministratorID { get; set; }

        public Bakery Bakery { get; set; }

        public int BakeryID { get; set; }

        public TruckDriver TruckDriver { get; set; }

        public string TruckDriverID { get; set; }

        public ICollection<OrderProducts> OrderProducts { get; set; }
    }
}