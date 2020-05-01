namespace FlourMill_1.Models
{
    public class AdminRate
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string RateDate { get; set; }
        public string RateText { get; set; }
        public Administrator Administrator { get; set; }

        public int AdministratorID { get; set; }
    }
}