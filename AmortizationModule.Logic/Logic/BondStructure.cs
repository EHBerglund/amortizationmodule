using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public class BondStructure
    {
        public AmortizationInput AddInstalments(AmortizationInput input)
        {
            List<AmortizationTransaction> newTransactions = new List<AmortizationTransaction>();
            foreach (DateTime term in input.AmortizationSecurity.Instalments.Keys)
            {
                AmortizationTransaction instalmentTransaction = input.AmortizationTransactions.FirstOrDefault(t => IsInstalment(t, term));
                if (instalmentTransaction == null)
                {
                    double faceValue = input.AmortizationTransactions.Where(t => t.TransactionDate < term && IsFaceValueChangeTransaction(t)).Sum(t => TransactionSetup.QuantityEffect(t));
                    if (faceValue != 0)
                    {
                        double rate = faceValue * input.AmortizationSecurity.Instalments[term];
                        instalmentTransaction = new AmortizationTransaction()
                        {
                            Quantity = 1,
                            Rate = Math.Abs(rate),
                            TransactionType = rate > 0 ? 68 : 44,
                            Currency = input.AmortizationSecurity.Currency,
                            CurrencyRate = 1,
                            Position = input.UserInput.PositionSeq,
                            TransactionDate = term,
                            TransactionSeq = -1
                        };
                        newTransactions.Add(instalmentTransaction);
                    }
                }
            }
            input.AmortizationTransactions.AddRange(newTransactions);
            
            return input;
        }

        private bool IsFaceValueChangeTransaction(AmortizationTransaction transaction)
        {
            List<int> faceValueChangeTransactions = new List<int>()
            {
                4, 67, 43, 40
            };
            bool isFaceValueChange = faceValueChangeTransactions.Contains(transaction.TransactionType);
            return isFaceValueChange;
        }

        private bool IsInstalment(AmortizationTransaction transaction, DateTime term)
        {
            List<int> instalmentTypes = new List<int>()
            {
                4, 41, 44, 67, 68, 43, 40, 6
            };
            bool isInstalment = transaction.TransactionDate == term && instalmentTypes.Contains(transaction.TransactionType);
            return isInstalment;
        }

        public AmortizationInput AddInterestTerms(AmortizationInput input)
        {
            Dictionary<DateTime, double> interestTerms = input.AmortizationSecurity.InterestTerms;
            Queue<Term> filteredInterestTerms = new Queue<Term>();
            DateTime minDate = input.AmortizationTransactions.Min(t => t.TransactionDate);
            foreach (DateTime term in interestTerms.Keys.Where(t => t > minDate).OrderByDescending(t => t))
            {
                AmortizationTransaction interestTransaction = input.AmortizationTransactions.FirstOrDefault(t => IsInterestTerm(t, term));
                if (interestTransaction == null)
                {
                    filteredInterestTerms.Enqueue(new Term(term, interestTerms[term]));
                }
            }
            List<AmortizationTransaction> newTransactions = new List<AmortizationTransaction>();

            double quantity = 0;
            foreach (AmortizationTransaction transaction in input.AmortizationTransactions)
            {
                try
                {
                    while (transaction.TransactionDate >= (filteredInterestTerms.Peek()?.TermDate ?? new DateTime(2100, 1, 1)))
                    {
                        Term term = filteredInterestTerms.Dequeue();
                        double rate = quantity * term.Amount;
                        if (rate != 0)
                        {
                            AmortizationTransaction newTransaction = new AmortizationTransaction()
                            {
                                Quantity = 1,
                                Rate = Math.Abs(rate),
                                TransactionType = rate > 0 ? 9 : 42,
                                TransactionDate = term.TermDate,
                                Position = transaction.Position,
                                Currency = input.AmortizationSecurity.Currency,
                                CurrencyRate = 1
                            };
                            newTransactions.Add(newTransaction);
                        }
                    }
                    quantity += TransactionSetup.QuantityEffect(transaction);
                }
                catch (InvalidOperationException e)
                {

                }
            }
            input.AmortizationTransactions.AddRange(newTransactions);

            return input;
        }

        private bool IsInterestTerm(AmortizationTransaction t, DateTime term)
        {
            List<int> termTypes = new List<int>()
            {
                9, 30, 42, 63
            };
            bool isTerm = t.TransactionDate == term && termTypes.Contains(t.TransactionType);
            return isTerm;
        }

        class Term
        {
            public DateTime TermDate { get; set; }
            public double Amount { get; set; }

            public Term(DateTime date, double amount)
            {
                TermDate = date;
                Amount = amount;
            }
        }
    }
}
