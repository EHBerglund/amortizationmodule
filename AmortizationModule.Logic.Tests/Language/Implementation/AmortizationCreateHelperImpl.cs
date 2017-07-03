using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic.Language.Implementation
{
    public class AmortizationCreateHelperImpl : AmortizationCreateHelper
    {
        public List<InterestRate> CreateInterestRates(Dictionary<string, double> InterestRates)
        {
            List<InterestRate> rates = new List<InterestRate>();
            foreach (string dateStr in InterestRates.Keys)
            {
                rates.Add(new InterestRate()
                {
                    Date = DateTime.Parse(dateStr),
                    Rate = InterestRates[dateStr]
                });
            }
            return rates;
        }

        public AmortizationSettings CreateSettings(int Method, bool OutputAggregated = false)
        {
            AmortizationSettings settings = new AmortizationSettings()
            {
                Method = Method,
                OutputAggregated = OutputAggregated
            };
            return settings;
        }

        public UserAmortizationInput CreateUserInput(string CalculationDate, int PositionSeq = 1)
        {
            UserAmortizationInput input = new UserAmortizationInput()
            {
                CalculationDate = DateTime.Parse(CalculationDate),
                PositionSeq = PositionSeq
            };
            return input;
        }

        public AmortizationSecurity Security(int SecurityType, string MaturityDate, int SecuritySeq = 1, bool Floater = false, string Currency = "NOK")
        {
            AmortizationSecurity security = new AmortizationSecurity()
            {
                SecuritySeq = SecuritySeq,
                SecurityType = SecurityType,
                MaturityDate = DateTime.Parse(MaturityDate),
                Floater = Floater,
                Currency = Currency
            };
            return security;
        }

        public AmortizationTransaction Transaction(string TransactionDate, int TransactionType, int Position, string Voucher, double Quantity, double Rate, int TransactionSeq = 1, string Currency = "NOK", double CurrencyRate = 1)
        {
            AmortizationTransaction transaction = new AmortizationTransaction()
            {
                TransactionDate = DateTime.Parse(TransactionDate),
                TransactionType = TransactionType,
                Position = Position,
                Voucher = Voucher,
                Quantity = Quantity,
                Rate = Rate,
                TransactionSeq = TransactionSeq,
                Currency = Currency,
                CurrencyRate = CurrencyRate
            };
            return transaction;
        }
    }
}
