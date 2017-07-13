using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class IssueLoanCategorizer : InitiationCategorizer
    {
        public void Initialize()
        {
        }

        public void ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return;

            if (transaction.TransactionType == 67)
            {
                initiations.Add(new IssueLoanInitiation(transaction));
                transaction.Categorized = true;
            }
        }
    }
}
