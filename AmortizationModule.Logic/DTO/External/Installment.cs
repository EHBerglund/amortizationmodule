using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.DTO.External
{
    public class Installment
    {
        public DateTime InstallmentDate { get; set; }
        public double InstallmentFactor { get; set; }
        public DateTime RegisteredDate { get; set; }
        public bool Active { get; set; }

        public Installment(DateTime installmentDate, double installmentFactor, DateTime registeredDate, bool active = true)
        {
            InstallmentDate = installmentDate;
            InstallmentFactor = installmentFactor;
            RegisteredDate = registeredDate;
            Active = active;
        }
    }
}
