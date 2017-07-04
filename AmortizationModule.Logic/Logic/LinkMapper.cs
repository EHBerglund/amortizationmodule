using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public static class LinkMapper
    {
        public static List<AmortizationInitiation> CoupleLinkToInitiation(List<AmortizationInitiation> initiations, AmortizationLink link, AmortizationSettings settings)
        {
            //Add check of method for mapping
            if (initiations.Count == 0)
                return initiations;
            DateTime leastDate = initiations.Min(i => i.TransactionDate);
            if (link.TransactionDate < leastDate)
            {
                initiations.OrderBy(i => i.TransactionDate).FirstOrDefault().AddLink(link);
                link.TransactionDate = leastDate;
                return initiations;
            }
            foreach (AmortizationInitiation init in initiations.OrderBy(i => i.TransactionDate))
            {
                if (init.TransactionDate <= link.TransactionDate)
                    init.AddLink(link);
            }
            return initiations;
        }
    }
}
