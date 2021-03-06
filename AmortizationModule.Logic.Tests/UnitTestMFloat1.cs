﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestBondsFloat1
    {
        AmortizationCreateHelper BuildHelper;
        AmortizationCommandHelper CommandHelper;
        AmortizationAssertHelper AssertHelper;
        [TestInitialize()]
        public void TestInitialize()
        {
            BuildHelper = Factory.CreateAmortizationCreateHelper();
            CommandHelper = Factory.CreateAmortizationCommandHelper();
            AssertHelper = Factory.CreateAmortizationAssertHelper();
        }


        private AmortizationInput SetUpBondsFloat1()
        {

            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "01.01.2019");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 2,
            MaturityDate: "01.01.2020",
            Floater: true,
            Currency: "USD");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod:1);

            Input.AmortizationSecurity.Instalments = new List<Installment>() {
                new Installment(new DateTime(2020,01,01), 1, new DateTime(2014,1,1)),
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2015,01,01), 0.047},
                { new DateTime(2016,01,01), 0.047},
                { new DateTime(2017,01,01), 0.0421150684931507},
                { new DateTime(2018,01,01), 0.042},
                { new DateTime(2019,01,01), 0.042},
                { new DateTime(2020,01,01), 0.042},
            };


            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
            { "01.01.2014",0.047},
            { "01.01.2016",0.042}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>()
            {
                BuildHelper.Transaction("01.01.2014",4,1,"V-01",1250000,0.96,1,"USD",5),
                BuildHelper.Transaction("01.01.2015",9,1,"V-02",58750,1,2,"USD",5),
                BuildHelper.Transaction("01.01.2016",9,1,"V-03",58750,1,3,"USD",5),
                BuildHelper.Transaction("01.01.2017",9,1,"V-04",52643.83562,1,4,"USD",5),
                BuildHelper.Transaction("01.01.2018",9,1,"V-05",52500,1,5,"USD",5),
                BuildHelper.Transaction("01.01.2019",9,1,"V-06",52500,1,6,"USD",5),
                BuildHelper.Transaction("01.01.2020",9,1,"V-07",52500,1,7,"USD",5),
                BuildHelper.Transaction("01.01.2020",68,1,"V-08",1250000,1,8,"USD",5)
            };

            return Input;
        }

        [TestMethod]
        public void TestBondFloat1()
        {
            AmortizationInput input = SetUpBondsFloat1();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("01.01.2019", 40582.68492, output);
        }
    }
}

