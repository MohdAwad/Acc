using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TrialBalanceVM
    {
        public int MonthNo { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public string CostCenter { get; set; }
        public string Name { get; set; }


        public double CreditBalance { get; set; }
        public double DebitBalance { get; set; }



        public double CreditTransAction { get; set; }
        public double DebitTransAction { get; set; }



        public double NetCredit { get; set; }
        public double NetDebit { get; set; }


        public string sCreditBalance { get; set; }
        public string sDebitBalance { get; set; }



        public string sCreditTransAction { get; set; }
        public string sDebitTransAction { get; set; }



        public string sNetCredit { get; set; }
        public string sNetDebit { get; set; }

        public int FatherAcc { get; set; }

        public int IsMainAccount { get; set; }

        public string  MainAccount { get; set; }

        public int Level { get; set; }



        public double nCreditBalance { get; set; }
        public double nDebitBalance { get; set; }



        public double nCreditTransAction { get; set; }
        public double nDebitTransAction { get; set; }



        public double nNetCredit { get; set; }
        public double nNetDebit { get; set; }


        public int Month { get; set; }


        public double January { get; set; }
        public double February { get; set; }
        public double March { get; set; }
        public double April { get; set; }
        public double May { get; set; }
        public double June { get; set; }
        public double July { get; set; }
        public double August { get; set; }
        public double September { get; set; }
        public double October { get; set; }
        public double November { get; set; }
        public double December { get; set; }




        public double nJanuary { get; set; }
        public double nFebruary { get; set; }
        public double nMarch { get; set; }
        public double nApril { get; set; }
        public double nMay { get; set; }
        public double nJune { get; set; }
        public double nJuly { get; set; }
        public double nAugust { get; set; }
        public double nSeptember { get; set; }
        public double nOctober { get; set; }
        public double nNovember { get; set; }
        public double nDecember { get; set; }


        public double OpenBalance { get; set; }
        public double nOpenBalance { get; set; }

        public double NetTot { get; set; }

        public double nNetTot { get; set; }

        public int CompanyTransactionKindNo { get; set; }



        public double MonthTransaction { get; set; }
        public double Expected { get; set; }

        public double ChangeVaule { get; set; }

        public double ChangePrec { get; set; }

        public double nMonthTransaction { get; set; }
        public double nExpected { get; set; }

        public double nChangeVaule { get; set; }

        public double nChangePrec { get; set; }




        public double MonthTransaction1 { get; set; }
        public double Expected1 { get; set; }
        public double ChangeVaule1 { get; set; }
        public double ChangePrec1 { get; set; }

       public double nMonthTransaction1 { get; set; }
        public double nExpected1 { get; set; }
        public double nChangeVaule1 { get; set; }
        public double nChangePrec1 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction2{ get; set; }
        public double Expected2 { get; set; }
        public double ChangeVaule2 { get; set; }
        public double ChangePrec2 { get; set; }

        public double nMonthTransaction2 { get; set; }
        public double nExpected2 { get; set; }
        public double nChangeVaule2 { get; set; }
        public double nChangePrec2 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction3 { get; set; }
        public double Expected3 { get; set; }
        public double ChangeVaule3 { get; set; }
        public double ChangePrec3 { get; set; }

        public double nMonthTransaction3 { get; set; }
        public double nExpected3 { get; set; }
        public double nChangeVaule3 { get; set; }
        public double nChangePrec3 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction4 { get; set; }
        public double Expected4 { get; set; }
        public double ChangeVaule4 { get; set; }
        public double ChangePrec4 { get; set; }

        public double nMonthTransaction4 { get; set; }
        public double nExpected4 { get; set; }
        public double nChangeVaule4 { get; set; }
        public double nChangePrec4 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction5 { get; set; }
        public double Expected5 { get; set; }
        public double ChangeVaule5 { get; set; }
        public double ChangePrec5 { get; set; }

        public double nMonthTransaction5 { get; set; }
        public double nExpected5 { get; set; }
        public double nChangeVaule5 { get; set; }
        public double nChangePrec5 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction6 { get; set; }
        public double Expected6 { get; set; }
        public double ChangeVaule6 { get; set; }
        public double ChangePrec6 { get; set; }

        public double nMonthTransaction6 { get; set; }
        public double nExpected6 { get; set; }
        public double nChangeVaule6 { get; set; }
        public double nChangePrec6 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction7 { get; set; }
        public double Expected7 { get; set; }
        public double ChangeVaule7 { get; set; }
        public double ChangePrec7 { get; set; }

        public double nMonthTransaction7 { get; set; }
        public double nExpected7 { get; set; }
        public double nChangeVaule7 { get; set; }
        public double nChangePrec7 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction8 { get; set; }
        public double Expected8 { get; set; }
        public double ChangeVaule8 { get; set; }
        public double ChangePrec8 { get; set; }

        public double nMonthTransaction8 { get; set; }
        public double nExpected8 { get; set; }
        public double nChangeVaule8 { get; set; }
        public double nChangePrec8 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction9 { get; set; }
        public double Expected9 { get; set; }
        public double ChangeVaule9 { get; set; }
        public double ChangePrec9 { get; set; }

        public double nMonthTransaction9 { get; set; }
        public double nExpected9 { get; set; }
        public double nChangeVaule9 { get; set; }
        public double nChangePrec9 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction10 { get; set; }
        public double Expected10 { get; set; }
        public double ChangeVaule10 { get; set; }
        public double ChangePrec10 { get; set; }

        public double nMonthTransaction10 { get; set; }
        public double nExpected10 { get; set; }
        public double nChangeVaule10 { get; set; }
        public double nChangePrec10 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction11 { get; set; }
        public double Expected11 { get; set; }
        public double ChangeVaule11 { get; set; }
        public double ChangePrec11 { get; set; }

        public double nMonthTransaction11 { get; set; }
        public double nExpected11 { get; set; }
        public double nChangeVaule11 { get; set; }
        public double nChangePrec11 { get; set; }

        //----------------------------------------------//
        public double MonthTransaction12 { get; set; }
        public double Expected12 { get; set; }
        public double ChangeVaule12 { get; set; }
        public double ChangePrec12 { get; set; }

        public double nMonthTransaction12 { get; set; }
        public double nExpected12 { get; set; }
        public double nChangeVaule12 { get; set; }
        public double nChangePrec12 { get; set; }

        //----------------------------------------------//

        public double ExpectedAnnually { get; set; }
        public double ActualTotal { get; set; }
        public double RemainingExpected { get; set; }
        public double RemainingRatio { get; set; }


        public double nExpectedAnnually { get; set; }
        public double nActualTotal { get; set; }
        public double nRemainingExpected { get; set; }
        public double nRemainingRatio { get; set; }



        //--------- For Search------------//
        public string TempCostID { get; set; }
        public int TempCostType { get; set; }

       


    }
}