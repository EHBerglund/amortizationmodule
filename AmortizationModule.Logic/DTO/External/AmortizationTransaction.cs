using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.DTO.External
{
    public class AmortizationTransaction
    {
        public int TransactionSeq { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionType { get; set; }
        public int Position { get; set; }
        public string Voucher { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public string Currency { get; set; }
        public double CurrencyRate { get; set; }
        public bool Categorized { get; set; }

        public AmortizationTransaction()
        {
            Categorized = false;
        }
    }
}
