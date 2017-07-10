using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.Internal;
using AmortizationModule.Logic.DTO.External;

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
        public double CalculateAccumulatedAmortization(AmortizationInitiation initiation, DateTime CalculationDate)
        {
            if (!input.AmortizationSecurity.Floater)
                return CalculateAccumulatedAmortizationInInterval(initiation, initiation.TransactionDate, CalculationDate);
            double accumulatedAmortization = 0;
            double initialCost = -faceValue + premiumDiscount;
            DateTime lastChangeDate = initiation.TransactionDate;
            foreach (InterestRate rateChangeDate in input.InterestRates.Where(r => r.Date < CalculationDate && r.Date > initiation.TransactionDate))
            {
                accumulatedAmortization += CalculateAccumulatedAmortizationInInterval(initiation, lastChangeDate, rateChangeDate.Date,initialCost - accumulatedAmortization, true);
                lastChangeDate = rateChangeDate.Date;
            }
            accumulatedAmortization += CalculateAccumulatedAmortizationInInterval(initiation, lastChangeDate, CalculationDate, initialCost - accumulatedAmortization, true);
            return accumulatedAmortization;
        }

        private double CalculateAccumulatedAmortizationInInterval(AmortizationInitiation initiation, DateTime fromDate, DateTime CalculationDate, double amortizedCost = 0, bool reCalculateInterest = false)
        {
            if (Math.Abs(amortizedCost) < 5)
                amortizedCost = -faceValue + premiumDiscount;
            if (reCalculateInterest)
                initiation = ReCalculateInterest(initiation, CalculationDate, input.InterestRates);

            return CalculateAccumulatedAmortizationInInterval(initiation, fromDate, CalculationDate, amortizedCost);
        }
        
        private double CalculateAccumulatedAmortizationInInterval(AmortizationInitiation initiation,DateTime fromDate, DateTime calculationDate, double amortizedCost)
        {
            double accumulatedAmortization = 0;
            IRR = CalculateIRR(initiation, fromDate, amortizedCost);
            double pvFutureCashFlows = 0;
            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate > calculationDate))
            {
                pvFutureCashFlows += CalculatePVSingleCashFlow(calculationDate, link.LinkDate, link.GetCashFlowAmount(), IRR);
            }
            accumulatedAmortization = Math.Abs(pvFutureCashFlows) - Math.Abs(amortizedCost);
            return accumulatedAmortization;
        }

        private AmortizationInitiation ReCalculateInterest(AmortizationInitiation initiation, DateTime calculationDate, List<InterestRate> interestRates)
        {
            double quantity = 0;
            DateTime interestDate = initiation.TransactionDate;
            double accruedInterest = 0;
            double rate = LookupRate(interestDate, interestRates);
            InterestLink link = null;
            DateTime endDate = initiation.Links.Max(l => l.LinkDate);
            while (interestDate <= endDate)
            {
                if (interestDate < calculationDate)
                {
                    rate = LookupRate(interestDate, interestRates);
                }
                accruedInterest += -CalculateOneDayInterest(quantity, rate);
                foreach (AmortizationLink termLink in initiation.Links.Where(l => l.LinkDate == interestDate))
                {
                    quantity += termLink.GetInstalmentAmount();
                }
                link = (InterestLink) initiation.Links.FirstOrDefault(l => l.GetType() == typeof(InterestLink) && l.LinkDate == interestDate);
                if (link != null)
                {
                    link.SetAmount(accruedInterest);
                    accruedInterest = 0;
                    link = null;
                }
                interestDate = interestDate.AddDays(1);
            }
            return initiation;
        }

        private double LookupRate(DateTime rateDate, List<InterestRate> interestRates)
        {
            double interest = interestRates.OrderBy(r => r.Date).FirstOrDefault()?.Rate ?? 0;
            foreach (InterestRate rate in interestRates.OrderBy(r => r.Date))
            {
                if (rate.Date <= rateDate)
                    interest = rate.Rate;
            }
            return interest;
        }

        private double CalculateOneDayInterest(double quantity, double rate)
        {
            return quantity * rate / 365;
        }

        private double CalculateIRR(AmortizationInitiation initiation, DateTime CalculationDate, double amortizedCost)
        {
            double accuracy = 0.0001;
            double highRate = 1;
            double lowRate = -0.9;
            double pv = double.MaxValue;
            double midPoint = 0;
            bool highEqualsPositive = false;

            double highPV = CalculatePV(CalculationDate, initiation, highRate, amortizedCost);
            double lowPV = CalculatePV(CalculationDate, initiation, lowRate, amortizedCost);

            if (highPV * lowPV >= 0)
                return 0;

            if (highPV > 0)
                highEqualsPositive = true;

            while (Math.Abs(pv) > accuracy)
            {
                midPoint = (highRate + lowRate) / 2;
                pv = CalculatePV(CalculationDate, initiation, midPoint, amortizedCost);
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

        private double CalculatePV(DateTime CalculationDate, AmortizationInitiation initiation, double discountRate, double timeZeroValue)
        {
            double pv = timeZeroValue;
            DateTime termDate;
            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate > CalculationDate).OrderBy(l => l.LinkDate))
            {
                termDate = link.LinkDate < CalculationDate ? CalculationDate : link.LinkDate;
                pv += CalculatePVSingleCashFlow(CalculationDate, termDate, link.GetCashFlowAmount(), discountRate);
            }
            return pv;
        }

        private double CalculatePVSingleCashFlow(DateTime CalculationDate, DateTime cfDate, double cf, double rate)
        {
            double daysGone = (cfDate - CalculationDate).TotalDays;
            double pv = cf / (Math.Pow(1 + rate, daysGone / 365));
            return pv;
        }
    }
}
