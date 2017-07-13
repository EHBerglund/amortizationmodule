using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class LoanIncomeCategorizer : LinkCategorizer
    {
        List<int> LoanIncomeTypes;
        public void Initialize()
        {
            LoanIncomeTypes = new List<int>() { 116 };
        }

        public List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return initiations;

            if (LoanIncomeTypes.Contains(transaction.TransactionType))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new LoanIncomeLink(transaction), input.Settings);
                transaction.Categorized = true;
            }
            return initiations;
        }
    }
}
