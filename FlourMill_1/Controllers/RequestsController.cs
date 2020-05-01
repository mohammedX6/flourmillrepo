using DatingApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly DataContext _context;

        public RequestsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get_requests")]
        public IActionResult getRequests()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = from r in _context.Order
                         where r.AdministratorID == id
                         select new
                         {
                             r.ID,
                             r.Order_Date,
                             r.TotalTons,
                             r.OrderStatues,
                             r.CustomerName,
                             r.Destination,
                             r.TotalPayment
                         };

            return Ok(result);
        }

        [HttpPost]
        [Route("change_request/{RID}")]
        public IActionResult ChangeRequest(int RID)
        {
            var getOrder = _context.Order.FirstOrDefault(x => x.ID == RID);
            getOrder.OrderStatues = 1;

            _context.Update(getOrder);
            _context.SaveChanges();

            var accountSid = "AC9385ee5b15020a0b41930222101b915e";
            var authToken = "b0adb5c55ca70e17ff56de32cfe3e364";
            TwilioClient.Init(accountSid, authToken);
            var gettruck = (from x in _context.TruckDriver
                            where x.AdministratorID == getOrder.AdministratorID
                            select new
                            {
                                x.PhoneNumber
                            }).ToList();

            for(int i=0;i<gettruck.Count;i++)
            {

                string truckphoneNumber = gettruck.ElementAt(i).PhoneNumber;
                var messageOptions = new CreateMessageOptions(
              new PhoneNumber("whatsapp:+962" + truckphoneNumber));
                messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                messageOptions.Body = "Order of " + getOrder.TotalTons + " tons has Accepted " + getOrder.Order_Date + " Please deliver it ASAP !";



                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
            }

   
    
           

          
      






            return Ok("Order Accepted");
        }

        [HttpPost]
        [Route("reject_request/{RID}")]
        public IActionResult DeleteRequest(int RID)
        {
            DateTime parsedDate;
            var getOrder = _context.Order.FirstOrDefault(x => x.ID == RID);

            var getbakery = _context.Bakery.FirstOrDefault(x => x.Id == getOrder.BakeryID);

            parsedDate = DateTime.Parse(getOrder.Order_Date);

            var accountSid = "AC9385ee5b15020a0b41930222101b915e";
            var authToken = "b0adb5c55ca70e17ff56de32cfe3e364";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:+962770133245"));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = "Your Flour order of " + getOrder.TotalTons + " tons has Canceled  " + parsedDate;

            var message = MessageResource.Create(messageOptions);

            var message2 = MessageResource.Create(messageOptions);

            _context.Order.Remove(getOrder);

            _context.SaveChanges();

            return Ok("Order Deleted");
        }
    }
}