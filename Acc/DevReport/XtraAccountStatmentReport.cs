using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Acc.DevReport
{
    public partial class XtraAccountStatmentReport : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraAccountStatmentReport()
        {
            InitializeComponent();
        }

        private void tableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string fieldName;
            XRTableCell cell = sender as XRTableCell;
            if (cell.DataBindings.Count > 0)
            {
                fieldName = cell.DataBindings["Debit"].DataMember;
                int index = fieldName.LastIndexOf(".");
                if (index > 0)
                    fieldName = fieldName.Substring(index + 1);
                double value = Convert.ToDouble(GetCurrentColumnValue(fieldName));
                if (value == 0)
                    cell.Text = "";
                else
                    cell.Text = String.Format("{0:n3}", value);
            }
        }
    }
}