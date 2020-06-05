using FlourMill_1.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckDriverManageController : ControllerBase
    {
        private readonly DataContext _context;

        public TruckDriverManageController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("addtruck/{JID}")]
        public async Task<IActionResult> addTruck(int JID)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string jobID = JID.ToString();
            var isFound = _context.TruckDriver.FirstOrDefault(x => x.JobNumber == jobID);

            if (isFound == null)
            {
                return BadRequest("Truck Driver not foud ");
            }

            isFound.AdministratorID = id;
            _context.TruckDriver.Update(isFound);
            await _context.SaveChangesAsync();
            return Ok("Truck Driver Added");
        }
    }
}