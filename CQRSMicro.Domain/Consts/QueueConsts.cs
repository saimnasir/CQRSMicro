using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSMicro.Domain.Consts
{
    public static class QueueConsts
    {
        public static string ProductCreated = nameof(ProductCreated);
        public static string ProductSold = nameof(ProductSold);
        public static string CustomerCreated = nameof(CustomerCreated); 
    }
}
