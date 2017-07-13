using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class LoanCostCategorizer : LinkCategorizer
    {
        private List<int> CostTypes;
        public void Initialize()
        {
            CostTypes = new List<int>() { 64, 92 };
        }

        public List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return initiations;

            if (CostTypes.Contains(transaction.TransactionType))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new LoanCostLink(transaction), input.Settings);
                transaction.Categorized = true;
            }
            return initiations;
        }
    }
}
