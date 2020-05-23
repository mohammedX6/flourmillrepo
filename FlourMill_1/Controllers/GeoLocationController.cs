using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoLocationController : ControllerBase
    {
        private readonly DataContext _context;

        public GeoLocationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getLocation/{id}")]
        public IActionResult getLocation(int id)
        {
            var geo = (from pd in _context.Bakery
                       where pd.Id == id
                       select new
                       {
                           pd.latitude,
                           pd.longitude
                       }).FirstOrDefault();

            string latittude = geo.latitude.ToString();
            string longitude = geo.longitude.ToString();

            return Ok(new
            {
                lati = latittude,
                longi = longitude
            });
        }
    }
}