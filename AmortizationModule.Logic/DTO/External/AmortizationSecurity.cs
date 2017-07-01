using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.DTO.External
{
    public class AmortizationSecurity
    {
        public int SecuritySeq { get; set; }
        public int SecurityType { get; set; }
        public DateTime MaturityDate { get; set; }
        public bool Floater { get; set; }
        public string Currency { get; set; }
    }
}
