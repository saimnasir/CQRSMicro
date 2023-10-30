using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSMicro.Domain
{
    public class BlockedNumbersConfig
    {
        public List<string> Numbers { get; set; } = new();
    }
}
