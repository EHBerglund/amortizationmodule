using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic.Language.Implementation
{
    public class AmortizationAssertHelperImpl : AmortizationAssertHelper
    {
        public void VerifyOutputTotalAccumulatedAmortizationEquals(string calculationDate, double accumulatedAmortization, AmortizationOutput output)
        {
            AmortizationTransactionOutput transaction = output.GetAccumulatedOutputTransaction();
            Assert.AreEqual(Math.Abs(accumulatedAmortization), transaction.Rate*transaction.Quantity, 5);
            if (accumulatedAmortization > 0)
            {
                Assert.AreEqual(117, transaction.TransactionType);
            }
            else if (accumulatedAmortization < 0)
            {
                Assert.AreEqual(118, transaction.TransactionType);
            }
        }
    }
}
