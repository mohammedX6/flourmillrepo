using FlourMill_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FlourMill_1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BakeryProductsController : ControllerBase
    {
        private readonly IDataRepository _repo;
        private readonly DataContext _context;

        public BakeryProductsController(DataContext context, IDataRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        [Route("{id}/{PID}")]
        public IActionResult GetSingleProduct(int id, int PID)
        {
            var td = (from pd in _context.Product
                      join od in _context.Administrator on id equals od.Id
                      where
                      pd.AdministratorID == od.Id
                      select new
                      {
                          pd.URL,
                          pd.BadgeName,
                          pd.BadgeType,
                          pd.BadgeSize,
                          pd.ProductionDate,
                          pd.ExpireDate,
                          pd.Usage,
                          pd.ProductDescription,
                          pd.price,
                          pd.ID
                      }).ToList().FirstOrDefault(x => x.ID == PID);
            return Ok(td);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAllProducts(int id)
        {
            var td = (from pd in _context.Product
                      join od in _context.Administrator on id equals od.Id
                      where
                      pd.AdministratorID == od.Id
                      select new
                      {
                          pd.URL,
                          pd.BadgeName,
                          pd.BadgeType,
                          pd.BadgeSize,
                          pd.ProductionDate,
                          pd.ExpireDate,
                          pd.Usage,
                          pd.ProductDescription,
                          pd.price,
                          pd.ID
                      }).ToList();

            return Ok(td);
        }

        [HttpGet]
        [Route("get_all")]
        public IActionResult getAllFlouMills()
        {
            var td = (from od in _context.Administrator

                      select new
                      {
                          od.JobNumber,
                          od.Username,
                          od.Email,
                          od.PhoneNumber,
                          od.Id
                      }).ToList();
            return Ok(td);
        }
    }
}