using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class InterestLink : AmortizationLink
    {
        public InterestLink(AmortizationTransaction transaction) 
            : base(transaction)
        {
        }

        public override double GetCashFlowAmount()
        {
            int transactionType = transaction.TransactionType;
            if (transactionType == 9 || transactionType == 30)
                return transaction.Rate * transaction.Quantity;
            if (transactionType == 42 || transactionType == 63)
                return -transaction.Rate * transaction.Quantity;
            return 0;
        }

        public override double GetInstalmentAmount()
        {
            if (transaction.TransactionType == 42)
                return -transaction.Rate * transaction.Quantity;
            if (transaction.TransactionType == 63)
                return transaction.Rate * transaction.Quantity;
            return 0;
        }
    }
}
