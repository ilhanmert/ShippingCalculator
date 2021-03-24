using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingCalculator.Entities.Concrete
{
    public class Corporation
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public decimal Discount { get; set; }
    }
}
