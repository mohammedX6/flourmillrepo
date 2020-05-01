using DatingApp.Data;
using FlourMill_1.Dtos;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;

        public OrdersController(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("addOrder_only")]
        public async Task<IActionResult> addOrderOnly(Order order)
        {

            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();

            var x = (from pd in _context.Order
                     join od in _context.Bakery on pd.BakeryID equals od.Id
                     select new
                     {
                         pd.ID
                     }).OrderByDescending(x => x.ID).First().ToString();




            var getbakery = _context.Administrator.FirstOrDefault(x => x.Id == order.AdministratorID);
            var gettruck = _context.TruckDriver.FirstOrDefault(x => x.Id == order.TruckDriverID);

            string phonnumber = getbakery.PhoneNumber;
            phonnumber = phonnumber.Remove(0, 1);

            string truckphoneNumber = getbakery.PhoneNumber;
            var accountSid = "AC9385ee5b15020a0b41930222101b915e";
            var authToken = "b0adb5c55ca70e17ff56de32cfe3e364";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:+962" + phonnumber));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = "Flour order of " + order.TotalTons + " tons has  Placed  " + order.Order_Date + " Please check it ASAP !";


            var message2 = MessageResource.Create(messageOptions);

            return Ok(new
            {

                id = x
            });

        }
        [HttpPost]
        [Route("addOrderProducts_only")]
        public async Task<IActionResult> addOrderProductsOnly(List<OrderProducts> order)
        {

            await _context.orderProducts.AddRangeAsync(order);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpPost]
        [Route("addOrder_full")]
        public async Task<IActionResult> addOrderFull(OrderFullDto orderFullDto)
        {
            Order myorder = orderFullDto.order;

            List<OrderProducts> orderProducts = orderFullDto.orderProducts;

            await _context.Order.AddAsync(myorder);
            await _context.SaveChangesAsync();
            await _context.orderProducts.AddRangeAsync(orderProducts);
            await _context.SaveChangesAsync();

            var x = (from pd in _context.Order
                     join od in _context.Bakery on pd.BakeryID equals od.Id
                     select new
                     {
                         pd.ID
                     }).OrderByDescending(x => x.ID).First().ToString();



            var getbakery = _context.Administrator.FirstOrDefault(x => x.Id == myorder.AdministratorID);
            var gettruck = _context.TruckDriver.FirstOrDefault(x => x.Id == myorder.TruckDriverID);

            string phonnumber = getbakery.PhoneNumber;
            phonnumber= phonnumber.Remove(0, 1);

            string truckphoneNumber = getbakery.PhoneNumber;
            var accountSid = "AC9385ee5b15020a0b41930222101b915e";
            var authToken = "b0adb5c55ca70e17ff56de32cfe3e364";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:+962" + phonnumber));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = "Flour order of " + myorder.TotalTons + " tons has  Placed  " + myorder.Order_Date+" Please check it ASAP !";


            var message2 = MessageResource.Create(messageOptions);
 

            return Ok(new
            {
                lod = x
            });
        }

        [HttpGet]
        [Route("getall/{id}")]
        public IActionResult GetAllOrders(int id)
        {
            var td = (from pd in _context.Order
                      where
                      pd.BakeryID == id
                      select new
                      {
                          pd.OrderComment,
                          pd.BakeryID,
                          pd.CustomerName,
                          pd.Destination,
                          pd.ShipmentPrice,
                          pd.TotalPayment,
                          pd.OrderStatues,
                          pd.Order_Date,
                          pd.TotalTons,
                          pd.ID
                      }).ToList();

            return Ok(td);
        }

        [HttpGet]
        [Route("get_balance/{id}")]
        public IActionResult getTruckDriverBalance(string id)
        {
            var value = (from pd in _context.Order
                         where pd.TruckDriverID == id
                         select new
                         {
                             pd.TotalPayment
                         }
                         ).ToList();

            double sum = 0;
            for (int i = 0; i < value.Count; i++)
            {
                sum += value.ElementAt(i).TotalPayment;
            }

            return Ok(new
            {
                balance = sum
            });
        }

        [HttpGet]
        [Route("getallTruck")]
        public IActionResult GetAllOrdersTruckDriver()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           
            var temp = _context.TruckDriver.FirstOrDefault(x => x.Id == id);
            int adminid = temp.AdministratorID;
            var td = (from pd in _context.Order

                      where pd.OrderStatues == 1 && pd.AdministratorID == adminid
                      select new
                      {
                          pd.OrderComment,
                          pd.BakeryID,
                          pd.CustomerName,
                          pd.Destination,
                          pd.ShipmentPrice,
                          pd.TotalPayment,
                          pd.OrderStatues,
                          pd.Order_Date,
                          pd.TotalTons,
                          pd.ID,
                          pd.TruckDriverID,
                          pd.AdministratorID
                      }).ToList();
            return Ok(td);
        }

        [HttpGet]
        [Route("gethistory")]
        public IActionResult Gethistory()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    
            var temp = _context.TruckDriver.FirstOrDefault(x => x.Id == id);
            int adminid = temp.AdministratorID;
          
            var td = (from pd in _context.Order

                      where pd.TruckDriverID== id && pd.OrderStatues == 1 && pd.AdministratorID == adminid || pd.OrderStatues == 2 || pd.OrderStatues == 3
                      select new
                      {
                          pd.OrderComment,
                          pd.BakeryID,
                          pd.CustomerName,
                          pd.Destination,
                          pd.ShipmentPrice,
                          pd.TotalPayment,
                          pd.OrderStatues,
                          pd.Order_Date,
                          pd.TotalTons,
                          pd.ID,
                          pd.TruckDriverID,
                          pd.AdministratorID
                      }).ToList();
            return Ok(td);
        }

        [HttpGet]
        [Route("GetOrderDetail/{id}")]
        public IActionResult GetOrderDetail(int id)
        {
            var td = (from od in _context.orderProducts
                      where
                      od.orderId == id
                      select new
                      {
                          od.pic,
                          od.price,
                          od.tons,
                          od.Badge,
                          od.orderId,
                          od.id,
                      }).ToList();

            return Ok(td);
        }

        [HttpGet]
        [Route("GetOrderDetailTruck/{id}")]
        public IActionResult GetOrderDetailTruckDriver(int id)
        {
            var td = (from od in _context.orderProducts
                      where
                      od.orderId == id
                      select new
                      {
                          od.pic,
                          od.price,
                          od.tons,
                          od.Badge,
                          od.orderId,
                          od.id,
                      }).ToList();

            return Ok(td);
        }

        [HttpPost]
        [Route("update_truck")]
        public async Task<IActionResult> UpdateTruck(UpdateTruckDTO updateTruck)
        {
            var entity2 = await _context.Order.FirstOrDefaultAsync(item => item.ID == updateTruck.orderid);
            var td = await (from od in _context.orderProducts
                            where od.orderId==updateTruck.orderid

                            select new OrderProducts
                            {
                               pic=  od.pic,
                               price= od.price,
                               tons= od.tons,
                               Badge= od.Badge,
                               orderId =od.orderId,
                              
                            }).ToListAsync();


            _context.Order.Remove(entity2);
            await _context.SaveChangesAsync();

            entity2.TruckDriverID = updateTruck.id;
            entity2.OrderStatues = updateTruck.orderStatues;
            Order o = new Order();
            o.AdministratorID = entity2.AdministratorID;
            o.BakeryID = entity2.BakeryID;
            o.CustomerName = entity2.CustomerName;
            o.Destination = entity2.Destination;
            o.OrderComment = entity2.OrderComment;
            o.OrderStatues = entity2.OrderStatues;
            o.TruckDriverID = entity2.TruckDriverID;
            o.TotalTons = entity2.TotalTons;
            o.TotalPayment = entity2.TotalPayment;
            o.Order_Date = entity2.Order_Date;
            o.ShipmentPrice = entity2.ShipmentPrice;

            await _context.Order.AddAsync(o);
            await _context.SaveChangesAsync();
            await _context.orderProducts.AddRangeAsync(td);
            await _context.SaveChangesAsync();

            return Ok("Tables Updated");
        }

        [HttpPost]
        [Route("finish_order")]
        public async Task<IActionResult> FinishOrder(FinishOrderDTO finishOrderDTO)
        {

          
            

            var beforeupdate = await _context.Order.FirstOrDefaultAsync(x => x.ID == finishOrderDTO.orderId);
 

            var td = await (from od in _context.orderProducts

                            where od.orderId == finishOrderDTO.orderId
                            select new OrderProducts
                            {
                                pic = od.pic,
                                price = od.price,
                                tons = od.tons,
                                Badge = od.Badge,
                                orderId = od.orderId,
                             
                            }).ToListAsync();


             _context.Order.Remove(beforeupdate);
           await _context.SaveChangesAsync();

         beforeupdate.OrderStatues = finishOrderDTO.orderStatues;
            Order o = new Order();
            o.AdministratorID = beforeupdate.AdministratorID;
            o.BakeryID = beforeupdate.BakeryID;
            o.CustomerName = beforeupdate.CustomerName;
            o.Destination = beforeupdate.Destination;
            o.OrderComment = beforeupdate.OrderComment;
            o.OrderStatues = beforeupdate.OrderStatues;
            o.TruckDriverID = beforeupdate.TruckDriverID;
            o.TotalTons = beforeupdate.TotalTons;
            o.TotalPayment = beforeupdate.TotalPayment;
            o.Order_Date = beforeupdate.Order_Date;
            o.ShipmentPrice = beforeupdate.ShipmentPrice;
            await _context.AddAsync(beforeupdate);
         await _context.orderProducts.AddRangeAsync(td);
         await _context.SaveChangesAsync();
            return Ok("Order Finished");
        }

        [HttpPost]
        [Route("add_order")]
        public async Task<ActionResult<Order>> PostOrder(List<Order> orders)
        {
            _context.Order.AddRange(orders);
            await _context.SaveChangesAsync();

            return Ok("Report added");
        }

        [HttpGet]
        [Route("checkTruck")]
        public IActionResult checkIfTruckDriverIsAvailable()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orderCheck = (from order in _context.Order
                              where order.TruckDriverID == id && order.OrderStatues == 2
                              select new
                              {
                                  order.OrderStatues
                              }).ToList();

            if (orderCheck.Count == 0)
            {
                return BadRequest("Truck driver not available");
            }
            else
            {
                return Ok(new
                {
                    flag = true
                });
            }
        }

        [HttpGet]
        [Route("getjob")]
        public IActionResult CheckTruckDriverJob()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var getOrder = (from order in _context.Order
                            where order.TruckDriverID == id && order.OrderStatues == 2
                            select new
                            {
                                order.AdministratorID,
                                order.BakeryID,
                                order.ID,
                                order.Destination
                            }).FirstOrDefault();

            if (getOrder == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(new
                {
                    adminid = getOrder.AdministratorID,
                    bakeryid = getOrder.BakeryID,
                    orderID = getOrder.ID,
                    dest = getOrder.Destination
                });
            }
        }
    }
}