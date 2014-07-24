using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Runtime.Caching;

namespace SampleApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Initialize the cache of fake sales
            var r = new Random();
            var users = new string[] { "Bob", "Tom", "Jack", "Dan", "Anne", "Kim" };
            var clients = new string[] { "Acme, Inc.", "THV, LLC", "Monster Electronics, LLC" };
            var widgets = new string[] { "Dohickey", "Gadget", "Doodad", "Whirleygig" };

            var sales = new List<SaleRecord>();
            var numRecords = r.Next(200) + 50;

            for (var i = 0; i < numRecords; i++)
            {
                sales.Add(new SaleRecord()
                {
                    Date = DateTime.Parse("2014/01/01").AddDays(r.Next(365)),
                    Id = Guid.NewGuid(),
                    Qty = r.Next(100),
                    Salesperson = users[r.Next(users.Length)],
                    Widget = widgets[r.Next(widgets.Length)],
                    Client = clients[r.Next(clients.Length)]
                });
            }

            MemoryCache.Default["sales"] = sales;
        }
    }
}