using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class AmortizationOutputImpl : AmortizationOutput
    {
        private List<AmortizationTransactionOutput> outputTransactions;
        public List<AmortizationTransactionOutput> GetListOfOutputTransactions()
        {
            throw new NotImplementedException();
        }

        public double GetTotalAmortizationReversalAmount()
        {
            throw new NotImplementedException();
        }

        public void AddTransactionsList(List<AmortizationTransactionOutput> outputTransactions)
        {
            this.outputTransactions = outputTransactions;
        }

        public AmortizationTransactionOutput GetAccumulatedOutputTransaction()
        {
            double positiveSum = outputTransactions.Where(t => t.TransactionType == (int)TransactionTypeDefs.AmortizationIncome).Sum(t => t.Rate * t.Quantity);
            double negativeSum = outputTransactions.Where(t => t.TransactionType == (int)TransactionTypeDefs.AmortizationCost).Sum(t => t.Rate * t.Quantity);
            return new AmortizationTransactionOutput()
            {
                PositionSeq = outputTransactions.FirstOrDefault().PositionSeq,
                Quantity = 1,
                Rate = Math.Abs(positiveSum - negativeSum),
                TransactionType = positiveSum > negativeSum ? (int)TransactionTypeDefs.AmortizationIncome : (int)TransactionTypeDefs.AmortizationCost
            };
        }
    }
}
