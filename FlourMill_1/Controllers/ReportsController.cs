using DatingApp.Data;
using FlourMill_1.Dtos;
using FlourMill_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlourMill_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IDataRepository _repo;
        private readonly DataContext _context;

        public ReportsController(DataContext context, IDataRepository repo)
        {
            _context = context;

            _repo = repo;
        }

        [HttpPost]
        [Route("generate_report")]
        public async Task<IActionResult> GenerateReport()
        {
            DateTime parsedDate;
            DateTime TempDate;
            DateTime After30date;
        int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string flourmillname = User.FindFirst(ClaimTypes.Name).Value;
            string username = User.FindFirst(ClaimTypes.Name).Value;
        
            var lastReport = _context.Report.FromSqlRaw("SELECT * FROM Report WHERE  Flour_Mill_Name={0}", flourmillname).ToList();
          
            if (lastReport.Count != 0)
            {

                string dateNew = lastReport.Select(x => x.Report_Date).Max();


                
                string ReportDate = dateNew;
                int adminidReport = lastReport[0].AdministratorID;

                parsedDate = DateTime.Parse(ReportDate);
                TempDate = parsedDate;
                After30date = TempDate.AddDays(29);
                DateTime dateTimeNow = DateTime.Now;
                if (dateTimeNow.Date >= After30date.Date || adminidReport != id)
                {
                    var getReports = await
                           (from admin in _context.Administrator
                            join order in _context.Order on id equals order.AdministratorID
                            where admin.Id == id 
                            select new
                            {
                                admin.Id,

                         
                                admin.Username,
                                admin.TotalFlourMillPayment,
                              

                                order.TotalTons,
                                order.Order_Date,
                                order.TotalPayment
                            }).ToListAsync();
                    double TotalBadges = 0;
                    double TotalMyPayment = 0;

                    string date = DateTime.Now.ToString("M/d/yyyy");
                    DateTime nowAfter30 = DateTime.Now.AddDays(29);
                    for (int i = 0; i < getReports.Count; i++)
                    {
                        if (DateTime.Parse(getReports[i].Order_Date).Date <= nowAfter30.Date)
                        {
                            TotalBadges += getReports[i].TotalTons;
                            TotalMyPayment += getReports[i].TotalPayment;
                        }
                    }
                    var report = new Report
                    {
                        AdministratorID = id,
                        SuperVisorID = 1,
                        TotalPayment = TotalMyPayment.ToString(),
                        TotalBadgesForFlourMill = TotalBadges,
                        Flour_Mill_Name = username
                        ,
                        Report_Date = date
                    };

                    _context.Report.Add(report);
                    await _context.SaveChangesAsync();
                    return Ok("Report Generated");
                }
            }
            else if (lastReport.Count == 0)
            {
                var getReports = await
                           (from admin in _context.Administrator
                            join product in _context.Product on id equals product.AdministratorID
                            join order in _context.Order on id equals order.AdministratorID
                            where admin.Id == id
                            select new
                            {
                                admin.Id,

                                product.Usage,
                                admin.Username,
                                admin.TotalFlourMillPayment,
                                product.BadgeName,
                                order.TotalTons,
                                order.TotalPayment,
                                order.Order_Date
                            }).ToListAsync();

           
                double TotalBadges = 0;
                double TotalMyPayment = 0;

                string date = DateTime.Now.ToString("M/d/yyyy");
                DateTime nowAfter30 = DateTime.Now.AddDays(29);

                for (int i = 0; i < getReports.Count; i++)
                {
                    if (DateTime.Parse(getReports[i].Order_Date).Date <= nowAfter30.Date)
                    {
                        TotalBadges += getReports[i].TotalTons;
                        TotalMyPayment += getReports[i].TotalPayment;
                    }
                }
                var report = new Report
                {
                    AdministratorID = id,
                    SuperVisorID = 1,
                    TotalPayment = TotalMyPayment.ToString(),
                    TotalBadgesForFlourMill = TotalBadges,
                    Flour_Mill_Name = username
                    ,
                    Report_Date = date
                };

                _context.Report.Add(report);
                await _context.SaveChangesAsync();
                return Ok("Report Generated");
            }
            return BadRequest("Report only generated after 30 days");
        }

        [HttpGet]
        [Route("get_report")]
        public async Task<ActionResult<IEnumerable<Report>>> GetReport()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

           return await _context.Report.Where(x => x.AdministratorID == id).ToListAsync();


        }


        [HttpGet]
        [Route("get_allreports")]
        public async Task<ActionResult<IEnumerable<Report>>> GetAllReport()
        {
          

            return await _context.Report.ToListAsync();


        }










        [HttpGet("{id}")]
        public  ActionResult<Report> GetReport(int id)
        {
            int id2 = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string username = User.FindFirst(ClaimTypes.Name).Value;

            var ReportDetaileds = from order in _context.Order
                                  from report in _context.Report
                                  join orderp in _context.orderProducts on order.ID equals orderp.orderId
                                  where report.ID == id && order.AdministratorID==id2
                                  select new
                                  {
                                      order.TotalTons,
                                      order.TotalPayment,
                                      orderp.Badge,
                                      

                                  } into t1
                                  group t1 by t1.Badge into g
                                  select new
                                  {
                                      Product = g.Key,
                                      Tons = g.Sum(x => x.TotalTons),
                                      Payment = g.Sum(x => x.TotalPayment),
                                  };
            return Ok(ReportDetaileds);
        }

       
        [HttpGet]
        [Route("single_reportsupervisor/{id}")]
        public ActionResult<Report> GetReportSupervisor(int id)
        {

            var getAdmin = _context.Report.Where(x => x.ID == id).Select(x2 => x2.AdministratorID).FirstOrDefault();
            int admin = getAdmin;
   

            var ReportDetaileds = from order in _context.Order
                                  from report in _context.Report
                                  join orderp in _context.orderProducts on order.ID equals orderp.orderId
                                  where report.ID == id && order.AdministratorID== admin
                                  select new
                                  {
                                      order.TotalTons,
                                      order.TotalPayment,
                                      orderp.Badge,


                                  } into t1
                                  group t1 by t1.Badge into g
                                  select new
                                  {
                                      Product = g.Key,
                                      Tons = g.Sum(x => x.TotalTons),
                                      Payment = g.Sum(x => x.TotalPayment),
                                  };
            return Ok(ReportDetaileds);
        }


        [HttpGet]
        [Route("last_report")]
        public IActionResult GetLastReport()
        {
            var lastReport2 = _context.Report.FromSqlRaw("SELECT * FROM Report WHERE id=(SELECT max(id) FROM Report);").ToList();

            string ReportDate = lastReport2[0].Report_Date.ToString();

            DateTime parsedDate = DateTime.Parse(ReportDate);
            DateTime TempDate = parsedDate;
            DateTime After30date = TempDate.AddDays(29);
            ReaminingDateDto reaminingDateDto = new ReaminingDateDto();
            reaminingDateDto.Mydate = Convert.ToInt32((After30date - parsedDate.Date).TotalDays);

            return Ok(reaminingDateDto);
        }

        [HttpPut]
        [Route("add_report")]
        public async Task<IActionResult> PutReport(Report report)
        {
            var addedreport = await _repo.AddReport(report);

            return Ok("Report added");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Report>> DeleteReport(int id)
        {
            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Report.Remove(report);
            await _context.SaveChangesAsync();

            return report;
        }
    }
}