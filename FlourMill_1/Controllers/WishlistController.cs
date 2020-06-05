using FlourMill_1.Data;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly DataContext _context;

        public WishlistController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetWishlist()
        {
            var td = await (from wish in _context.Wishlist
                            select new
                            {
                                wish.id,
                                wish.BakeryId,
                                wish.AdministratorId,
                                wish.ProductId,
                                wish.Badgename,
                                wish.price
                                ,
                                wish.url
                            }).ToListAsync();

            return Ok(td);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Wishlist>> GetWishlist(int id)
        {
            var wishlist = await _context.Wishlist.FindAsync(id);

            if (wishlist == null)
            {
                return NotFound();
            }

            return wishlist;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddWishList(Wishlist wishlist)
        {
            await _context.Wishlist.AddAsync(wishlist);

            await _context.SaveChangesAsync();

            return Ok("Wishlist added");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Wishlist>> DeleteWishlist(int id)
        {
            var wishlist = await _context.Wishlist.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }

            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();

            return wishlist;
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.id == id);
        }
    }
}