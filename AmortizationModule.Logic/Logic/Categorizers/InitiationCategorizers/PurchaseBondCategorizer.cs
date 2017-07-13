using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class PurchaseBondCategorizer : InitiationCategorizer
    {
        public void Initialize()
        {
        }

        public void ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return;
            if (transaction.TransactionType == (int)TransactionTypeDefs.Purchase && !input.AmortizationSecurity.IsIssue)
            {
                initiations.Add(new PurchaseInitiation(transaction));
                transaction.Categorized = true;
            }
        }
    }
}
