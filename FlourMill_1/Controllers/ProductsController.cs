using FlourMill_1.Data;
using FlourMill_1.Dtos;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDataRepository _repo;
        private readonly DataContext _context;

        public ProductsController(DataContext context, IDataRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        [Route("{id}/{PID}")]
        public IActionResult GetSingleProduct(int id, int PID)
        {
            if (id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
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
            return Unauthorized();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAllProducts(int id)
        {
            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == id)
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
            return Unauthorized();
        }

        [HttpPost]
        [Route("add_product")]
        public async Task<IActionResult> addpro(ProductDto addProductDto)
        {
            if (addProductDto.AdministratorID == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                addProductDto.BadgeType = addProductDto.BadgeType.ToLower();
                if (await _repo.ProductExist(addProductDto.ID))
                {
                    return BadRequest("Product already inserted");
                }
                var CreateProduct = new Product
                {
                    URL = addProductDto.URL,
                    BadgeName = addProductDto.BadgeName,
                    BadgeType = addProductDto.BadgeType,
                    BadgeSize = addProductDto.BadgeSize,
                    ProductionDate = addProductDto.ProductionDate,
                    ExpireDate = addProductDto.ExpireDate,
                    Usage = addProductDto.Usage,
                    ProductDescription = addProductDto.ProductDescription,
                    price = addProductDto.price,
                    AdministratorID = addProductDto.AdministratorID
                };

                var InsertProduct = await _repo.AddNewProduct(CreateProduct);

                return Ok(new { InsertProduct = "Product added!" });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("delete_product")]
        public async Task<IActionResult> delpro(Product productDto)
        {
            if (productDto.AdministratorID == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                if (productDto.ID.ToString() == null)
                {
                    return BadRequest("Please enter the product id");
                }

                if (await _repo.ProductExist(productDto.ID) == false)
                {
                    return BadRequest("Product not found");
                }

                await _repo.DeleteProduct(productDto.ID);

                return Ok("Product deleted");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("update_product/{id}")]
        public IActionResult UpdateProduct(int id, Product productDto)

        {
            if (id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                var entity = _context.Product.FirstOrDefault(item => item.ID == productDto.ID);

                if (entity != null)
                {
                    entity.price = productDto.price;
                    entity.ProductionDate = productDto.ProductionDate;
                    entity.ExpireDate = productDto.ExpireDate;
                    entity.ProductDescription = productDto.ProductDescription;
                    entity.BadgeName = productDto.BadgeName;

                    _context.Product.Update(entity);

                    _context.SaveChanges();
                    return Ok("Product updated");
                }
                else
                {
                    return BadRequest("Error in request");
                }
            }
            return Unauthorized();
        }
    }
}