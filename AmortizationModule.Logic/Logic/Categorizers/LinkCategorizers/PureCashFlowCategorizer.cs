using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class PureCashFlowCategorizer : LinkCategorizer
    {
        private List<int> PositivePureCashFlowTypes;
        private List<int> NegativePureCashFlowTypes;
        public void Initialize()
        {
            PositivePureCashFlowTypes = new List<int>() { };
            NegativePureCashFlowTypes = new List<int>() { };
        }

        public List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return initiations;

            if (PositivePureCashFlowTypes.Contains(transaction.TransactionType))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new PureCashFlowLink(transaction, true), input.Settings);
                transaction.Categorized = true;
            }
            else if (NegativePureCashFlowTypes.Contains(transaction.TransactionType))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new PureCashFlowLink(transaction, false), input.Settings);
            }

            return initiations;
        }
    }
}
