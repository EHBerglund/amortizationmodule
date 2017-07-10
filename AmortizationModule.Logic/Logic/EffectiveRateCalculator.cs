using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class EffectiveRateCalculator : AmortizationCalculator
    {
        private double IRR;
        private double premiumDiscount;
        private double faceValue;
        AmortizationInput input;

        public void Initialize(AmortizationInput input, AmortizationInitiation initiation)
        {
            this.input = input;
            faceValue = initiation.GetFaceValue();
            premiumDiscount = initiation.GetPremiumDiscount();
        }
        public double CalculateAccumulatedAmortization(AmortizationInitiation initiation, DateTime calculationDate)
        {
            double accumulatedAmortization = 0;
            IRR = CalculateIRR(initiation);
            double pvFutureCashFlows = 0;
            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate > calculationDate))
            {
                pvFutureCashFlows += CalculatePVSingleCashFlow(calculationDate, link.LinkDate, link.GetCashFlowAmount(), IRR);
            }
            accumulatedAmortization = Math.Abs(pvFutureCashFlows) - Math.Abs(faceValue - premiumDiscount);
            return accumulatedAmortization;
        }

        private double CalculateIRR(AmortizationInitiation initiation)
        {
            DateTime calculationDate = initiation.TransactionDate;
            return CalculateIRR(initiation, calculationDate);
        }

        private double CalculateIRR(AmortizationInitiation initiation, DateTime CalculationDate)
        {
            double accuracy = 0.0001;
            double highRate = 1;
            double lowRate = -0.9;
            double pv = double.MaxValue;
            double midPoint = 0;
            bool highEqualsPositive = false;

            double highPV = CalculatePV(CalculationDate, initiation, highRate);
            double lowPV = CalculatePV(CalculationDate, initiation, lowRate);

            if (highPV * lowPV >= 0)
                return 0;

            if (highPV > 0)
                highEqualsPositive = true;

            while (Math.Abs(pv) > accuracy)
            {
                midPoint = (highRate + lowRate) / 2;
                pv = CalculatePV(CalculationDate, initiation, midPoint);
                if ((pv>0 && highEqualsPositive) || (pv < 0 && !highEqualsPositive))
                {
                    highRate = midPoint;
                }
                else
                {
                    lowRate = midPoint;
                }
            }
            return midPoint;
        }

        private double CalculatePV(DateTime CalculationDate, AmortizationInitiation initiation, double discountRate)
        {
            double pv = 0;
            DateTime termDate;
            foreach (AmortizationLink link in initiation.Links.OrderBy(l => l.LinkDate))
            {
                termDate = link.LinkDate < CalculationDate ? CalculationDate : link.LinkDate;
                pv += CalculatePVSingleCashFlow(CalculationDate, termDate, link.GetCashFlowAmount(), discountRate);
            }
            return pv;
        }

        private double CalculatePVSingleCashFlow(DateTime CalculationDate, DateTime cfDate, double cf, double rate)
        {
            double daysGone = (cfDate - CalculationDate).TotalDays;
            return cf / (Math.Pow(1 + rate, daysGone / 365));
        }
    }
}
