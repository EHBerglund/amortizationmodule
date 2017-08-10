using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmortizationModule.Logic
{
    public abstract class AmortizationInitiation
    {
        private AmortizationTransaction transaction;
        private List<int> issueTypes;
        private List<int> purchaseTypes;
        public DateTime TransactionDate
        {
            get
            {
                return transaction.TransactionDate;
            }
            set
            {
                transaction.TransactionDate = value;
            }
        }

        public string Currency
        {
            get
            {
                return transaction.Currency;
            }
        }

        public double CurrencyRate
        {
            get
            {
                return transaction.CurrencyRate;
            }
        }

        private List<AmortizationLink> links;
        public List<AmortizationLink> Links
        {
            get
            {
                return links;
            }
            set
            {
                links = value;
            }
        }
        public AmortizationInitiation(AmortizationTransaction transaction)
        {
            this.transaction = transaction;
            double premium = (1 - transaction.Rate) * transaction.Quantity;
            if (premium != 0)
                AddLink(new PremiumDiscountLink(transaction));

            AddLink(new InitiationLink(transaction));

            issueTypes = new List<int>() { 40, 43 };
            purchaseTypes = new List<int>() { 4, 67 };
        }

        public void AddLink(AmortizationLink link)
        {
            if (links == null)
                links = new List<AmortizationLink>();
            links.Add(link);
        }

        public virtual double GetPremiumDiscount()
        {
            return links.Sum(l => l.GetPremiumDiscountAmount());
        }

        public virtual double GetFaceValue()
        {
            if (issueTypes.Contains(transaction.TransactionType))
                return -transaction.Quantity;
            if (purchaseTypes.Contains(transaction.TransactionType))
                return transaction.Quantity;
            return 0;
        }
    }
}
