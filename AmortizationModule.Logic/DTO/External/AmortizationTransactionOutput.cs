using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.DTO.External
{
    public class AmortizationTransactionOutput
    {
        public int PositionSeq { get; set; }
        public int TransactionType { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public string Currency { get; set; }
        public double CurrencyRate { get; set; }
    }
}
