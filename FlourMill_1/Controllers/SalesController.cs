using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly DataContext _context;

        public SalesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get_sales")]
        public IActionResult getSalesPayment()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var sales = from order in _context.Order
                        join orderp in _context.orderProducts on order.ID equals orderp.orderId
                        where order.AdministratorID == id
                        select new
                        {
                            order.TotalPayment,
                            order.TotalTons,
                            orderp.Badge,
                            orderp.price,
                            orderp.orderId
                            ,orderp.tons,
                    
                        } into t1
                        group t1 by t1.Badge into g
                        select new
                        {
                            Product = g.Key,              
                            Payment = g.Sum(x => x.price * x.tons)
                        };

            return Ok(sales);
        }

        [HttpGet]
        [Route("get_tons")]
        public IActionResult getSalesTons()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var sales = from order in _context.Order
                        join orderp in _context.orderProducts on order.ID equals orderp.orderId
                        where order.AdministratorID == id
                        select new
                        {
                            order.TotalTons,
                            orderp.Badge,
                            orderp.orderId

                        } into t1
                        group t1 by t1.Badge into g
                        select new
                        {
                            Product = g.Key,
                            Tons = g.Sum(x => x.TotalTons)
                        };

            return Ok(sales);
        }

        [HttpGet]
        [Route("get_salessupervisor")]
        public IActionResult Get_FlourMills_Sales()
        {
            var sales =
                        from order in _context.Order
                        join admin in _context.Administrator on order.AdministratorID equals admin.Id

                        select new
                        {
                            admin.Username,
                            order.TotalPayment,
                        } into t1
                        group t1 by t1.Username into g
                        select new
                        {
                            Product = g.Key,
                            Payment = g.Sum(x => x.TotalPayment)
                        };

            return Ok(sales);
        }
    }
}