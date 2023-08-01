using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSMicro.Domain.Models
{
    public class ProductSoldModel
    {
        public Guid Id { get; set; }
        public int QuantitySold{ get; set; }
    }
}
