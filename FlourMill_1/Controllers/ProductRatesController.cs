using DatingApp.Data;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRatesController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductRatesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductRate>>> GetProductRate()
        {
            return await _context.ProductRate.ToListAsync();
        }

        [HttpGet]
        [Route("getBakeryRate/{pid}")]
        public async Task<IActionResult> GetProductRate(int pid)
        {
            var td = await (from rate in _context.ProductRate
                            where
                            rate.ProductID == pid
                            select new
                            {
                                rate.Id,
                                rate.BakeryId,
                                rate.AdministratorID,
                                rate.RateDate,
                                rate.RateText,
                                rate.ProductID,
                                rate.Value
                            }).ToListAsync();
            return Ok(td);
        }

        [HttpGet]
        [Route("getusersrates/{pid}")]
        public async Task<IActionResult> GetAllRates(int pid)
        {
            var td = await (from pd in _context.ProductRate
                            join od in _context.Bakery on pd.BakeryId equals od.Id
                            where
                            pd.ProductID == pid
                            select new
                            {
                                pd.Id,
                                pd.Value,
                                od.Username,
                                pd.RateDate,
                                pd.RateText
                            }).ToListAsync();
            return Ok(td);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductRate(int id, ProductRate productRate)
        {
            if (id != productRate.Id)
            {
                return BadRequest();
            }

            _context.Entry(productRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductRateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Route("add_rate")]
        public async Task<ActionResult<ProductRate>> PostProductRate(ProductRate productRate)
        {
            _context.ProductRate.Add(productRate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductRate", new { id = productRate.Id }, productRate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductRate>> DeleteProductRate(int id)
        {
            var productRate = await _context.ProductRate.FindAsync(id);
            if (productRate == null)
            {
                return NotFound();
            }

            _context.ProductRate.Remove(productRate);
            await _context.SaveChangesAsync();

            return productRate;
        }

        private bool ProductRateExists(int id)
        {
            return _context.ProductRate.Any(e => e.Id == id);
        }
    }
}