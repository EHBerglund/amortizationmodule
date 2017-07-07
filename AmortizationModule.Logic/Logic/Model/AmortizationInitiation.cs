using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public abstract class AmortizationInitiation
    {
        private AmortizationTransaction transaction;
        public DateTime TransactionDate { get => transaction.TransactionDate; set => transaction.TransactionDate = value; }

        private List<AmortizationLink> links;
        public List<AmortizationLink> Links { get => links; set => links = value; }
        public AmortizationInitiation(AmortizationTransaction transaction)
        {
            double premium = (1 - transaction.Rate) * transaction.Quantity;
            if (premium != 0)
                AddLink(new PremiumDiscountLink(transaction));
        }

        public void AddLink(AmortizationLink link)
        {
            if (links == null)
                links = new List<AmortizationLink>();
            links.Add(link);
        }

        public double GetTotalCost()
        {
            return links.Sum(l => l.GetPremiumDiscountAmount());
        }

        public double GetFaceValue()
        {
            return transaction.Quantity;
        }
    }
}
