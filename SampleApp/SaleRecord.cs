using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleApp
{
    public class SaleRecord
    {
        public Guid Id { get; set; }
        public string Salesperson { get; set; }
        public string Widget { get; set; }
        public int Qty { get; set; }
        public string Client { get; set; }
        public DateTime Date { get; set; }
    }
}