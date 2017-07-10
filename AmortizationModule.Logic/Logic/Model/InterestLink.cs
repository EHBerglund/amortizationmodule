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
        private double CurrentAmount;
        private double Amount;
        public InterestLink(AmortizationTransaction transaction) 
            : base(transaction)
        {
            int transactionType = transaction.TransactionType;
            if (transactionType == 9 || transactionType == 30)
                CurrentAmount = transaction.Rate * transaction.Quantity;
            if (transactionType == 42 || transactionType == 63)
                CurrentAmount = -transaction.Rate * transaction.Quantity;
            Amount = CurrentAmount;
        }

        public override double GetCashFlowAmount()
        {
            return Amount;
        }

        public override double GetInstalmentAmount()
        {
            if (transaction.TransactionType == 30 || transaction.TransactionType == 63)
                return Amount;
            return 0;
        }

        public void SetAmount(double amount)
        {
            Amount = amount;
        }

        public void ResetToCurrent()
        {
            Amount = CurrentAmount;
        }
    }
}
