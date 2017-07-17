using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public interface AmortizationOutput
    {
        List<AmortizationTransactionOutput> GetListOfOutputTransactions();
        double GetTotalAmortizationReversalAmount();
        void AddTransactionsList(List<AmortizationTransactionOutput> outputTransactions);

        AmortizationTransactionOutput GetAccumulatedOutputTransaction();
    }
}
