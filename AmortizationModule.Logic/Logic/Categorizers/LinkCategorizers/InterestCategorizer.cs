using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class InterestCategorizer : LinkCategorizer
    {
        private List<int> interestTypes;
        public void Initialize()
        {
            interestTypes = new List<int>() { 9, 30, 42, 63 };
        }

        public List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return initiations;

            if (interestTypes.Contains(transaction.TransactionType))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new InterestLink(transaction), input.Settings);
            }
            return initiations;
        }
    }
}
