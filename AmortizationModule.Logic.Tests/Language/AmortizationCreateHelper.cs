using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.Language
{
    public interface AmortizationCreateHelper
    {
        UserAmortizationInput CreateUserInput(string CalculationDate,int PositionSeq = 1);
        AmortizationSecurity Security(int SecurityType, string MaturityDate, int SecuritySeq = 1, bool Floater=false, string Currency="NOK");
        List<InterestRate> CreateInterestRates(Dictionary<string, double> InterestRates);
        AmortizationSettings CreateSettings(int Method, bool OutputAggregated = false);
        List<AmortizationTransaction> CreateTransactions(List<AmortizationTransaction> Transactions);
        AmortizationTransaction Transaction(string TransactionDate,int TransactionType, int Position, string Voucher, 
            double Quantity, double Rate, int TransactionSeq = 1, string Currency = "NOK", double CurrencyRate = 1);
    }
}
