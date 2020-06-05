using FlourMill_1.Data;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageAccountsController : ControllerBase
    {
        private readonly IDataRepository _repo;

        private readonly DataContext _context;

        public ManageAccountsController(IDataRepository repo, DataContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet]
        [Route("get_allAdmin")]
        public IActionResult getAllFlouMills()
        {
            var td = (from od in _context.Administrator

                      select new
                      {
                          od.Username,
                          od.Email,
                          od.Id
                      }).ToList();
            return Ok(td);
        }

        [HttpGet]
        [Route("get_allBakery")]
        public IActionResult getAllBakery()
        {
            var td = (from od in _context.Bakery

                      select new
                      {
                          od.Username,
                          od.Email,
                          od.Id
                      }).ToList();
            return Ok(td);
        }

        [HttpGet]
        [Route("get_allTruck")]
        public IActionResult getAllTruck()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var td = (from od in _context.TruckDriver
                      where od.AdministratorID == id
                      select new
                      {
                          od.Username,
                          od.Email,
                          od.Id
                      }).ToList();
            return Ok(td);
        }

        [HttpGet]
        [Route("get_allTruckFull")]
        public IActionResult getAllTruckFull()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var td = (from od in _context.TruckDriver
                      where od.AdministratorID == id
                      select new
                      {
                          od.Username,
                          od.Email,
                          od.Id,
                          od.PhoneNumber,
                          od.JobNumber,
                          od.NationalId
                      }).ToList();
            return Ok(td);
        }

        [HttpDelete]
        [Route("del_admin/{id}")]
        public async Task<IActionResult> delAccountAdmin(int id)
        {
            var account = await this._context.Administrator.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            this._context.Administrator.Remove(account);
            await _context.SaveChangesAsync();
            return Ok("Account deleted successfully");
        }

        [HttpDelete]
        [Route("del_bakery/{id}")]
        public async Task<IActionResult> delAccountBakery(int id)
        {
            var account2 = await _context.Bakery.FindAsync(id);
            if (account2 == null)
            {
                return NotFound();
            }
            _context.Bakery.Remove(account2);
            await _context.SaveChangesAsync();
            return Ok("Account deleted successfully");
        }

        [HttpDelete]
        [Route("del_truck/{id}")]
        public async Task<IActionResult> delAccountTruck(string id)
        {
            var accountTruck = await this._context.TruckDriver.FindAsync(id);

            var listOfOrdersTruckDriverAssigned = await (from o in _context.Order
                                                         where id == o.TruckDriverID
                                                         select new Order
                                                         {
                                                             ID = o.ID,
                                                             AdministratorID = o.AdministratorID,
                                                             TotalTons = o.TotalTons,
                                                             BakeryID = o.BakeryID,
                                                             CustomerName = o.CustomerName,
                                                             Destination = o.Destination,
                                                             Order_Date = o.Order_Date,
                                                             OrderComment = o.OrderComment,
                                                             OrderStatues = o.OrderStatues,
                                                             ShipmentPrice = o.ShipmentPrice,
                                                             TotalPayment = o.TotalPayment,
                                                             TruckDriverID = o.TruckDriverID
                                                         }).ToListAsync();

            List<Order> beforeUpdate = listOfOrdersTruckDriverAssigned;

            for (int i = 0; i < beforeUpdate.Count; i++)
            {
                beforeUpdate[i].TruckDriverID = "1";
            }
            _context.Order.UpdateRange(beforeUpdate);
            this._context.TruckDriver.Remove(accountTruck);
            await this._context.SaveChangesAsync();
            return Ok("Account deleted successfully");
        }

        [HttpGet]
        [Route("get_truck_info/{id}")]
        public IActionResult getTruckInfo(string id)
        {
            var checkIfTruckDriverHaveJobs = _context.Order.FirstOrDefault(x => x.TruckDriverID == id);

            if (checkIfTruckDriverHaveJobs != null)
            {
                var allInfo = (from tr in _context.TruckDriver
                               join od in _context.Order on tr.Id equals od.TruckDriverID
                               where tr.Id == id
                               select new
                               {
                                   tr.Id,
                                   tr.Username,
                                   tr.Email,
                                   tr.BirthDate,
                                   tr.JobNumber,
                                   tr.NationalId,
                                   tr.PhoneNumber,
                                   od.TotalPayment,
                                   od.TotalTons
                               }).ToList();

                double payment = 0;
                double tons = 0;
                for (int i = 0; i < allInfo.Count; i++)
                {
                    payment += allInfo.ElementAt(i).TotalPayment;
                    tons += allInfo.ElementAt(i).TotalTons;
                }

                var AllInfo = new
                {
                    allInfo.ElementAt(0).Id,
                    allInfo.ElementAt(0).Username,
                    allInfo.ElementAt(0).Email,
                    allInfo.ElementAt(0).BirthDate,
                    allInfo.ElementAt(0).PhoneNumber,
                    allInfo.ElementAt(0).JobNumber,
                    allInfo.ElementAt(0).NationalId,
                    payment,
                    tons
                };

                return Ok(AllInfo);
            }
            else
            {
                var allInfo = (from tr in _context.TruckDriver

                               where tr.Id == id
                               select new
                               {
                                   tr.Id,
                                   tr.Username,
                                   tr.Email,
                                   tr.BirthDate,
                                   tr.JobNumber,
                                   tr.NationalId,
                                   tr.PhoneNumber
                               }).ToList();

                double payment = 0;
                double tons = 0;

                var AllInfo = new
                {
                    allInfo.ElementAt(0).Id,
                    allInfo.ElementAt(0).Username,
                    allInfo.ElementAt(0).Email,
                    allInfo.ElementAt(0).BirthDate,
                    allInfo.ElementAt(0).PhoneNumber,
                    allInfo.ElementAt(0).JobNumber,
                    allInfo.ElementAt(0).NationalId,
                    payment,
                    tons
                };

                return Ok(AllInfo);
            }
        }

        [HttpPost]
        [Route("update_Truck")]
        public IActionResult UpdateProduct(TruckDriver truck)

        {
            var entity = _context.TruckDriver.FirstOrDefault(t => t.Id == truck.Id);

            if (entity != null)
            {
                entity.PhoneNumber = truck.PhoneNumber;
                entity.NationalId = truck.NationalId;
                entity.JobNumber = truck.JobNumber;

                _context.TruckDriver.Update(entity);

                _context.SaveChanges();
                return Ok("Truck Driver updated");
            }
            else
            {
                return BadRequest("Truck Driver not updated");
            }
        }
    }
}