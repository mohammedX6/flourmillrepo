namespace FlourMill_1.Models
{
    public class Report
    {
        public int ID { get; set; }
        public string Report_Date { get; set; }
        public string Flour_Mill_Name { get; set; }

        public string TotalPayment { get; set; }

        public double TotalBadgesForFlourMill { get; set; }

        public Administrator Administrator { get; set; }

        public int AdministratorID { get; set; }

        public SuperVisor SuperVisor { get; set; }

        public int SuperVisorID { get; set; }
    }
}