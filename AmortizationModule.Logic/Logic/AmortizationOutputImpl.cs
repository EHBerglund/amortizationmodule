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

        public double GetTotalAccumulatedAmortizationAmount()
        {
            return outputTransactions.Sum(t => t.Rate * t.Quantity);
        }

        public double GetTotalAmortizationReversalAmount()
        {
            throw new NotImplementedException();
        }

        public void AddTransactionsList(List<AmortizationTransactionOutput> outputTransactions)
        {
            this.outputTransactions = outputTransactions;
        }
    }
}
