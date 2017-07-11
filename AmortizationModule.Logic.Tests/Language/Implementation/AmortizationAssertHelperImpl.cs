using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmortizationModule.Logic.Language.Implementation
{
    public class AmortizationAssertHelperImpl : AmortizationAssertHelper
    {
        public void VerifyOutputTotalAccumulatedAmortizationEquals(string calculationDate, double accumulatedAmortization, AmortizationOutput output)
        {
            Assert.AreEqual(accumulatedAmortization, output.GetTotalAccumulatedAmortizationAmount(), 5);
        }
    }
}
