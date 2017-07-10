using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class InterestCategorizer : LinkCategorizer
    {
        public void Initialize()
        {
        }

        public List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            List<int> interestTypes = new List<int>() { 9, 30, 42, 63 };
            if (interestTypes.Contains(transaction.TransactionType))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new InterestLink(transaction), input.Settings);
            }
            return initiations;
        }
    }
}
