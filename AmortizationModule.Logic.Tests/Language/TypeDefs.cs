using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.Language
{
    public enum SecurityTypeDefs
    {
        Bond = 2,
        Loan = 23
    }

    public enum TransactionTypeDefs
    {
        Purchase = 4,
        Payback = 68
    }

    public enum AmortizationMethodDefs
    {
        EfficientRateMethod = 1,
        LinearMethod = 2
    }
}
