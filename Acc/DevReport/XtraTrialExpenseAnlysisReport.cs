using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Acc.DevReport
{
    public partial class XtraTrialExpenseAnlysisReport : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraTrialExpenseAnlysisReport()
        {
            InitializeComponent();
        }

        private void tableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string currValue = this.GetCurrentColumnValue("MainAccount").ToString();
            if (currValue == "{*}")
            {
                XRTableRow row = sender as XRTableRow;

                row.BackColor = Color.Yellow;
            }
            else
            {
                XRTableRow row = sender as XRTableRow;
                row.BackColor = Color.Transparent;

            }
        }
    }
}
