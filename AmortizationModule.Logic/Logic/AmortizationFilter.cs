using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public class AmortizationFilter
    {
        public AmortizationInput SetBasicMeasuresAndFilterIrrelevantTransactions(AmortizationInput input)
        {
            List<int> IncludedTransactionTypes = CreateIncludedTransactionsList();
            List<AmortizationTransaction> newTransactionsList = new List<AmortizationTransaction>();
            foreach (AmortizationTransaction transaction in input.AmortizationTransactions.OrderBy(t => t.TransactionDate))
            {
                if (transaction.TransactionType == (int)TransactionTypeDefs.Issue)
                    input.AmortizationSecurity.IsIssue = true;
                if (IncludedTransactionTypes.Contains(transaction.TransactionType))
                    newTransactionsList.Add(transaction);
            }
            input.AmortizationTransactions = newTransactionsList;
            return input;
        }

        private List<int> CreateIncludedTransactionsList()
        {
            return new List<int>()
            {
                1, 2, 4, 6, 9, 11, 30, 38, 40, 41, 42, 43, 44, 63, 64, 65, 66, 67, 68,
                92, 117, 118, 121
            };
        }

    }
}
