using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.Internal;
using AmortizationModule.Logic.BusinessLogic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class EffectiveRateCalculator : AmortizationCalculator
    {
        private AmortizationInitiation initiation;
        private AmortizationInput input;
        private double initialCost;
        public void Initialize(AmortizationInput input, AmortizationInitiation initiation)
        {
            this.input = input;
            this.initiation = initiation;
            this.initialCost = -initiation.GetFaceValue() + initiation.GetPremiumDiscount();
        }

        public double CalculateAccumulatedAmortization(DateTime calculationDate)
        {
            AmortizationParameters parameters = new AmortizationParameters();
            parameters.CalculationDate = calculationDate;
            parameters.CostAtCalculationDate = initialCost;
            parameters.InitialDate = initiation.TransactionDate;
            parameters.InitialCost = initialCost;

            double accruedAmortization = 0;
            double calculatedAmortization = 0;
            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate < calculationDate && l.LinkDate > initiation.TransactionDate).OrderBy(l => l.LinkDate))
            {
                parameters.CostAtCalculationDate += link.GetInstalmentAmount();
                if (link.TriggerRecalculationWithIRR)
                {
                    parameters.CalculationDate = link.LinkDate;
                    ReCalculateInterest(parameters.CalculationDate, input.InterestRates);
                    calculatedAmortization = RecalculateIRRAndCalculateAccumulatedAmortization(parameters);
                    accruedAmortization += calculatedAmortization;
                    parameters.CostAtCalculationDate -= calculatedAmortization;
                    parameters.InitialCost = parameters.CostAtCalculationDate;
                    parameters.InitialDate = link.LinkDate;
                }
                else if (link.TriggerRecalculationWithOldIRR)
                {
                }
            }

            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate == calculationDate && l.LinkDate != parameters.InitialDate))
            {
                parameters.CostAtCalculationDate += link.GetInstalmentAmount();
            }
            parameters.CalculationDate = calculationDate;
            ReCalculateInterest(parameters.CalculationDate, input.InterestRates);
            calculatedAmortization = RecalculateIRRAndCalculateAccumulatedAmortization(parameters);
            accruedAmortization += calculatedAmortization;
            return accruedAmortization;
        }

        private double RecalculateIRRAndCalculateAccumulatedAmortization(AmortizationParameters parameters)
        {
            parameters.IRR = CalculateIRR(parameters.InitialDate, parameters.InitialCost);
            return CalculateAmortizationInInterval(parameters);
        }

        private double CalculateAccumulatedAmortizationWithExistingIRR(AmortizationParameters parameters)
        {
            return CalculateAmortizationInInterval(parameters);
        }

        private AmortizationInitiation ReCalculateInterest(DateTime calculationDate, List<InterestRate> interestRates)
        {
            double quantity = 0;
            DateTime interestDate = initiation.TransactionDate;
            double accruedInterest = 0;
            double rate = LookupRate(interestDate, interestRates,calculationDate);
            InterestLink link = null;
            DateTime endDate = initiation.Links.Max(l => l.LinkDate);
            while (interestDate <= endDate)
            {
                if (interestDate < calculationDate)
                {
                    rate = LookupRate(interestDate, interestRates, calculationDate);
                }
                accruedInterest += -CalculateOneDayInterest(quantity, rate);
                foreach (AmortizationLink termLink in initiation.Links.Where(l => l.LinkDate == interestDate))
                {
                    quantity += termLink.GetInstalmentAmount();
                }
                link = (InterestLink)initiation.Links.FirstOrDefault(l => l.GetType() == typeof(InterestLink) && l.LinkDate == interestDate);
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

        private double LookupRate(DateTime rateDate, List<InterestRate> interestRates, DateTime calculationDate)
        {
            double interest = interestRates.OrderBy(r => r.Date).FirstOrDefault()?.Rate ?? 0;
            foreach (InterestRate rate in interestRates.OrderBy(r => r.Date))
            {
                if (rate.Date <= rateDate && rate.Date < calculationDate)
                    interest = rate.Rate;
            }
            return interest;
        }

        private double CalculateOneDayInterest(double quantity, double rate)
        {
            return quantity * rate / 365;
        }

        private double CalculateIRR(DateTime CalculationDate, double InitialCost)
        {
            double highRate = IRRParameters.highRate;
            double lowRate = IRRParameters.lowRate;
            double midpoint = IRRParameters.midpoint;

            double pv = 1;

            bool highEqualsPositive = ThrowIfNotSuitableForIRRAndDetermineIfHighRateYieldsPositiveNPV(CalculationDate, highRate, lowRate, InitialCost);

            while(Math.Abs(pv) > IRRParameters.accuracy)
            {
                midpoint = (highRate + lowRate) / 2;
                pv = CalculatePresentValueOfFutureCashFlows(CalculationDate, midpoint, InitialCost);
                if ((pv > 0 && highEqualsPositive) || (pv<0 && !highEqualsPositive))
                {
                    highRate = midpoint;
                }
                else
                {
                    lowRate = midpoint;
                }
            }

            return midpoint;
        }

        private bool ThrowIfNotSuitableForIRRAndDetermineIfHighRateYieldsPositiveNPV(DateTime CalculationDate, double highRate, double lowRate, double InitialCost)
        {
            double highPV = CalculatePresentValueOfFutureCashFlows(CalculationDate, highRate, InitialCost);
            double lowPV = CalculatePresentValueOfFutureCashFlows(CalculationDate, lowRate, InitialCost);

            if (CashFlowNotSuitableForIRR(highPV, lowPV))
                throw new CashFlowNotSuitableForIRRCalculation($"High: {highPV}, Low: {lowPV}");

            return highPV > 0;
        }

        private bool CashFlowNotSuitableForIRR(double highPV, double lowPV)
        {
            return highPV * lowPV > 0;
        }

        private double CalculateAmortizationInInterval(AmortizationParameters parameters)
        {
            double pvFutureCashFlows = CalculatePresentValueOfFutureCashFlows(parameters.CalculationDate, parameters.IRR);
            double accumulatedAmortization = pvFutureCashFlows + parameters.CostAtCalculationDate;
            return accumulatedAmortization;
        }

        private double CalculatePresentValueOfFutureCashFlows(DateTime CalculationDate, double rate, double initialValue = 0)
        {
            double pvFutureCashFlows = initialValue;
            foreach (AmortizationLink link in initiation.Links.Where(l => l.LinkDate > CalculationDate)) 
            {
                pvFutureCashFlows += CalculatePVSingleCashFlow(CalculationDate, link.LinkDate, link.GetCashFlowAmount(), rate);
            }
            return pvFutureCashFlows;
        }

        private double CalculatePVSingleCashFlow(DateTime CalculationDate, DateTime cfDate, double cf, double rate)
        {
            double daysGone = (cfDate - CalculationDate).TotalDays;
            double pv = cf / (Math.Pow(1 + rate, daysGone / 365));
            return pv;
        }
    }

    class AmortizationParameters
    {
        public DateTime InitialDate { get; set; }
        public DateTime CalculationDate { get; set; }
        public double InitialCost { get; set; }
        public double CostAtCalculationDate { get; set; }
        public double IRR { get; set; }
    }

    class IRRParameters
    {
        public const double accuracy = 0.0001;
        public const double highRate = 1;
        public const double lowRate = -0.9;
        public const double pv = double.MaxValue;
        public const double midpoint = 0;
    }
}
