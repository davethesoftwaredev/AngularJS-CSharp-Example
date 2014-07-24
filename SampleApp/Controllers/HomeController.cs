using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        Random rand = new Random();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View("Partials/Dashboard");
        }

        public ActionResult Person()
        {
            return View("Partials/Person");
        }

        public ActionResult Sales()
        {
            // Simulate report compilation delay/network delay
            System.Threading.Thread.Sleep(rand.Next(2000) + 1000);

            // sales are randomly generated and cached in the ApplicationStart in Global.asax
            // so note that they are always volatile and change when the app pool spins up
            // (this is not a database or localstorage sample!)
            var result = new JsonNetResult();
            var data = ((List<SaleRecord>)MemoryCache.Default["sales"]).GroupBy(x => x.Salesperson);

            result.Data = data.Select(x => new
            {
                Salesperson = x.Key,
                Count = x.Select(y => y).Count(),
                Widgets = x.Select(y => y.Qty).Sum()
            }).OrderBy(x => x.Salesperson);

            return result;
        }

        public ActionResult PersonReport(string id)
        {
            // Simulate report compilation delay/network delay
            System.Threading.Thread.Sleep(rand.Next(2000) + 1000);

            // sales are randomly generated and cached in the ApplicationStart in Global.asax
            // so note that they are always volatile and change when the app pool spins up
            // (this is not a database or localstorage sample!)
            var result = new JsonNetResult();
            result.Data = ((List<SaleRecord>)MemoryCache.Default["sales"]).Where(x => x.Salesperson == id).OrderBy(x => x.Date);
            return result;
        }

        public ActionResult MonthReport()
        {
            // Simulate report compilation delay/network delay
            System.Threading.Thread.Sleep(rand.Next(2000) + 1000);

            var months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var dataset = new List<int>();
            var result = new JsonNetResult();

            // sales are randomly generated and cached in the ApplicationStart in Global.asax
            // so note that they are always volatile and change when the app pool spins up
            // (this is not a database or localstorage sample!)            
            List<SaleRecord> sales = (List<SaleRecord>)MemoryCache.Default["sales"];

            for(var month = 0; month < months.Length; month++)
            {
                dataset.Add(sales.Where(x => x.Date.Month == month + 1).Sum(x => x.Qty));
            }

            result.Data = new
            {
                labels = months,
                datasets = new object[]
                {
                    new {
                        data = dataset
                    }
                }
            };

            return result;
        }
    }
}