using System.ComponentModel.DataAnnotations;

namespace FlourMill_1.Dtos
{
    public class ProductDto
    {
        public int ID { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public string BadgeName { get; set; }

        [Required]
        public string BadgeType { get; set; }

        [Required]
        public string BadgeSize { get; set; }

        [Required]
        public string ProductionDate { get; set; }

        [Required]
        public string ExpireDate { get; set; }

        [Required]
        public string Usage { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        public int price { get; set; }

        [Required]
        public int AdministratorID { get; set; }
    }
}