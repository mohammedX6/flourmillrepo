using FlourMill_1.Models;
using System.Collections.Generic;

namespace FlourMill_1.Dtos
{
    public class OrderFullDto
    {
        public Order order { get; set; }
        public List<OrderProducts> orderProducts { get; set; }
    }
}