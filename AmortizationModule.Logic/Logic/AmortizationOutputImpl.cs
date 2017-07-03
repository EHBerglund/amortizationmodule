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
        public List<AmortizationTransactionOutput> GetListOfOutputTransactions()
        {
            throw new NotImplementedException();
        }

        public double GetTotalAccumulatedAmortizationAmount()
        {
            return 2992;
        }

        public double GetTotalAmortizationReversalAmount()
        {
            throw new NotImplementedException();
        }
    }
}
