using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.Internal;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.BusinessLogic;

namespace AmortizationModule.Logic
{
    public class EffectiveRateCalculatorImpl : AmortizationCalculator
    {
        private double OriginalIRR;
        private double CurrentIRR;
        private double premiumDiscount;
        private double faceValue;
        private double initialCost;
        AmortizationInput input;
        AmortizationInitiation initiation;

        public void Initialize(AmortizationInput input, AmortizationInitiation initiation)
        {
            this.input = input;
            this.initiation = initiation;
            faceValue = initiation.GetFaceValue();
            premiumDiscount = initiation.GetPremiumDiscount();
            initialCost = -faceValue + premiumDiscount;
        }

        public double CalculateAccumulatedAmortization(DateTime CalculationDate)
        {
            double amortization = 0;
            
            return amortization;
        }

        public double CalculateAccumulatedAmortizationImpl(DateTime CalculationDate)
        {
            double accumulatedAmortization = 0;
            DateTime lastChangeDate = initiation.TransactionDate;
            double amortizedCost = initialCost - accumulatedAmortization;

            if (!input.AmortizationSecurity.Floater)
            {
                amortizedCost = initialCost - initiation.Links.Where(l => l.LinkDate <= CalculationDate).Sum(l => l.GetInstalmentAmount());
                return CalculateAccumulatedAmortizationInIntervalIRRBeginning(initiation.TransactionDate, CalculationDate, amortizedCost, initialCost);
            }

            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate > initiation.TransactionDate).OrderBy(l => l.LinkDate))
            {
                amortizedCost -= link.GetInstalmentAmount();
                if (link.LinkDate < CalculationDate && link.LinkDate > initiation.TransactionDate && link.TriggerRecalculationWithIRR)
                {
                    accumulatedAmortization += CalculateAccumulatedAmortizationInInterval(lastChangeDate, link.LinkDate, amortizedCost, true);
                    lastChangeDate = link.LinkDate;
                }
            }
            accumulatedAmortization += CalculateAccumulatedAmortizationInInterval(lastChangeDate, CalculationDate, initialCost - accumulatedAmortization, true);
            return accumulatedAmortization;
        }

        private double CalculateAccumulatedAmortizationInIntervalIRRBeginning(DateTime fromDate, DateTime CalculationDate, double amortizedCost, double initialCost)
        {
            CurrentIRR = CalculateIRR(fromDate, initialCost);
            return CalculateAccumulatedAmortizationInInterval(fromDate, CalculationDate, amortizedCost, CurrentIRR);
        }

        private double CalculateAccumulatedAmortizationInInterval(DateTime fromDate, DateTime CalculationDate, double amortizedCost = 0, bool reCalculateInterest = false)
        {
            if (Math.Abs(amortizedCost) < 5)
                amortizedCost = -faceValue + premiumDiscount;
            if (reCalculateInterest)
                initiation = ReCalculateInterest(CalculationDate, input.InterestRates);

            CurrentIRR = CalculateIRR(fromDate, amortizedCost);

            return CalculateAccumulatedAmortizationInInterval(fromDate, CalculationDate, amortizedCost, CurrentIRR);
        }
        
        private double CalculateAccumulatedAmortizationInInterval(DateTime fromDate, DateTime calculationDate, double amortizedCost, double IRR)
        {
            double accumulatedAmortization = 0;
            double pvFutureCashFlows = 0;
            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate > calculationDate))
            {
                pvFutureCashFlows += CalculatePVSingleCashFlow(calculationDate, link.LinkDate, link.GetCashFlowAmount(), IRR);
            }
            accumulatedAmortization = Math.Abs(pvFutureCashFlows) - Math.Abs(amortizedCost);
            return accumulatedAmortization;
        }

        private AmortizationInitiation ReCalculateInterest(DateTime calculationDate, List<InterestRate> interestRates)
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

        private double CalculateIRR(DateTime CalculationDate, double amortizedCost)
        {
            double accuracy = 0.0001;
            double highRate = 1;
            double lowRate = -0.9;
            double pv = double.MaxValue;
            double midPoint = 0;
            bool highEqualsPositive = false;

            double highPV = CalculatePV(CalculationDate, highRate, amortizedCost);
            double lowPV = CalculatePV(CalculationDate, lowRate, amortizedCost);

            if (highPV * lowPV >= 0)
                throw new CashFlowNotSuitableForIRRCalculation($"High: {highPV}, Low: {lowPV}");

            if (highPV > 0)
                highEqualsPositive = true;

            while (Math.Abs(pv) > accuracy)
            {
                midPoint = (highRate + lowRate) / 2;
                pv = CalculatePV(CalculationDate, midPoint, amortizedCost);
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

        private double CalculatePV(DateTime CalculationDate, double discountRate, double timeZeroValue)
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

        private double CalculateAmortizationInInterval(AmortizationParameters parameters)
        {
            double accumulatedAmortization = 0;
            double pvFutureCashFlows = 0;

            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate > parameters.CalculationDate))
            {
                pvFutureCashFlows += CalculatePVSingleCashFlow(parameters.CalculationDate, link.LinkDate, link.GetCashFlowAmount(), parameters.IRR);
            }
            accumulatedAmortization = pvFutureCashFlows - parameters.CostAtCalculationDate;
            return accumulatedAmortization;
        }

        class AmortizationParameters
        {
            public DateTime InitialDate { get; set; }
            public DateTime CalculationDate { get; set; }
            public double CostAtCalculationDate { get; set; }
            public double IRR { get; set; }
        }
    }
}
