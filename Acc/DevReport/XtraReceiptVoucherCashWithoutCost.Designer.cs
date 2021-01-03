namespace Acc.DevReport
{
    partial class XtraReceiptVoucherCashWithoutCost
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.LbPrintTime = new DevExpress.XtraReports.UI.XRLabel();
            this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.LbAccFromPaidName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.LbVoucherNo = new DevExpress.XtraReports.UI.XRLabel();
            this.LbVDate = new DevExpress.XtraReports.UI.XRLabel();
            this.LbAccPaidFrom = new DevExpress.XtraReports.UI.XRLabel();
            this.LbDebitAccNO = new DevExpress.XtraReports.UI.XRLabel();
            this.LbAccDebitName = new DevExpress.XtraReports.UI.XRLabel();
            this.LbSaleMan = new DevExpress.XtraReports.UI.XRLabel();
            this.LbNOte = new DevExpress.XtraReports.UI.XRLabel();
            this.LbNetTot = new DevExpress.XtraReports.UI.XRLabel();
            this.LbReporttitle = new DevExpress.XtraReports.UI.XRLabel();
            this.LbCoName = new DevExpress.XtraReports.UI.XRLabel();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.LbTafqet = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 8.333333F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LbPrintTime,
            this.pageInfo1,
            this.pageInfo2});
            this.BottomMargin.Name = "BottomMargin";
            // 
            // LbPrintTime
            // 
            this.LbPrintTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.LbPrintTime.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 44.70835F);
            this.LbPrintTime.Multiline = true;
            this.LbPrintTime.Name = "LbPrintTime";
            this.LbPrintTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbPrintTime.SizeF = new System.Drawing.SizeF(194.0007F, 22.99999F);
            this.LbPrintTime.StylePriority.UseForeColor = false;
            this.LbPrintTime.StylePriority.UseTextAlignment = false;
            this.LbPrintTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // pageInfo1
            // 
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            // 
            // pageInfo2
            // 
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(331F, 6F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.pageInfo2.TextFormatString = "Page {0} of {1}";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LbTafqet,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel4,
            this.LbAccFromPaidName,
            this.xrLabel6,
            this.xrLabel8,
            this.xrLabel9,
            this.xrLabel10,
            this.LbVoucherNo,
            this.LbVDate,
            this.LbAccPaidFrom,
            this.LbDebitAccNO,
            this.LbAccDebitName,
            this.LbSaleMan,
            this.LbNOte,
            this.LbNetTot,
            this.LbReporttitle,
            this.LbCoName});
            this.ReportHeader.HeightF = 341.25F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel2
            // 
            this.xrLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(527.4999F, 108.9583F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel2.StylePriority.UseBackColor = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "رقم القيد";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(206.5F, 108.9583F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "التاريخ";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(527.4999F, 144.2917F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel4.StylePriority.UseBackColor = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "حساب المدفوع منه";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbAccFromPaidName
            // 
            this.LbAccFromPaidName.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbAccFromPaidName.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbAccFromPaidName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbAccFromPaidName.LocationFloat = new DevExpress.Utils.PointFloat(23.91663F, 144.2917F);
            this.LbAccFromPaidName.Multiline = true;
            this.LbAccFromPaidName.Name = "LbAccFromPaidName";
            this.LbAccFromPaidName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbAccFromPaidName.SizeF = new System.Drawing.SizeF(282.5833F, 23F);
            this.LbAccFromPaidName.StylePriority.UseBorderDashStyle = false;
            this.LbAccFromPaidName.StylePriority.UseBorders = false;
            this.LbAccFromPaidName.StylePriority.UseFont = false;
            this.LbAccFromPaidName.StylePriority.UseTextAlignment = false;
            this.LbAccFromPaidName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(527.4999F, 183.4583F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "حساب الصندوق";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(527.4999F, 223.6667F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "المندوب";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel9
            // 
            this.xrLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(527.4999F, 259.5833F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "ملاحظة";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(212)))), ((int)(((byte)(246)))));
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(527.4999F, 298.125F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel10.StylePriority.UseBackColor = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "الصافي";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbVoucherNo
            // 
            this.LbVoucherNo.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbVoucherNo.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbVoucherNo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbVoucherNo.LocationFloat = new DevExpress.Utils.PointFloat(344.5417F, 108.9583F);
            this.LbVoucherNo.Multiline = true;
            this.LbVoucherNo.Name = "LbVoucherNo";
            this.LbVoucherNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbVoucherNo.SizeF = new System.Drawing.SizeF(170.0417F, 23F);
            this.LbVoucherNo.StylePriority.UseBorderDashStyle = false;
            this.LbVoucherNo.StylePriority.UseBorders = false;
            this.LbVoucherNo.StylePriority.UseFont = false;
            this.LbVoucherNo.StylePriority.UseTextAlignment = false;
            this.LbVoucherNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbVDate
            // 
            this.LbVDate.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbVDate.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbVDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbVDate.LocationFloat = new DevExpress.Utils.PointFloat(23.91663F, 108.9583F);
            this.LbVDate.Multiline = true;
            this.LbVDate.Name = "LbVDate";
            this.LbVDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbVDate.SizeF = new System.Drawing.SizeF(170.0417F, 23F);
            this.LbVDate.StylePriority.UseBorderDashStyle = false;
            this.LbVDate.StylePriority.UseBorders = false;
            this.LbVDate.StylePriority.UseFont = false;
            this.LbVDate.StylePriority.UseTextAlignment = false;
            this.LbVDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.LbVDate.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // LbAccPaidFrom
            // 
            this.LbAccPaidFrom.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbAccPaidFrom.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbAccPaidFrom.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbAccPaidFrom.LocationFloat = new DevExpress.Utils.PointFloat(344.5417F, 144.2917F);
            this.LbAccPaidFrom.Multiline = true;
            this.LbAccPaidFrom.Name = "LbAccPaidFrom";
            this.LbAccPaidFrom.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbAccPaidFrom.SizeF = new System.Drawing.SizeF(170.0416F, 23F);
            this.LbAccPaidFrom.StylePriority.UseBorderDashStyle = false;
            this.LbAccPaidFrom.StylePriority.UseBorders = false;
            this.LbAccPaidFrom.StylePriority.UseFont = false;
            this.LbAccPaidFrom.StylePriority.UseTextAlignment = false;
            this.LbAccPaidFrom.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbDebitAccNO
            // 
            this.LbDebitAccNO.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbDebitAccNO.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbDebitAccNO.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbDebitAccNO.LocationFloat = new DevExpress.Utils.PointFloat(344.5417F, 183.4583F);
            this.LbDebitAccNO.Multiline = true;
            this.LbDebitAccNO.Name = "LbDebitAccNO";
            this.LbDebitAccNO.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbDebitAccNO.SizeF = new System.Drawing.SizeF(170.0416F, 23F);
            this.LbDebitAccNO.StylePriority.UseBorderDashStyle = false;
            this.LbDebitAccNO.StylePriority.UseBorders = false;
            this.LbDebitAccNO.StylePriority.UseFont = false;
            this.LbDebitAccNO.StylePriority.UseTextAlignment = false;
            this.LbDebitAccNO.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbAccDebitName
            // 
            this.LbAccDebitName.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbAccDebitName.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbAccDebitName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbAccDebitName.LocationFloat = new DevExpress.Utils.PointFloat(23.91663F, 183.4583F);
            this.LbAccDebitName.Multiline = true;
            this.LbAccDebitName.Name = "LbAccDebitName";
            this.LbAccDebitName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbAccDebitName.SizeF = new System.Drawing.SizeF(282.5833F, 23F);
            this.LbAccDebitName.StylePriority.UseBorderDashStyle = false;
            this.LbAccDebitName.StylePriority.UseBorders = false;
            this.LbAccDebitName.StylePriority.UseFont = false;
            this.LbAccDebitName.StylePriority.UseTextAlignment = false;
            this.LbAccDebitName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbSaleMan
            // 
            this.LbSaleMan.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbSaleMan.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbSaleMan.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbSaleMan.LocationFloat = new DevExpress.Utils.PointFloat(23.91663F, 223.6667F);
            this.LbSaleMan.Multiline = true;
            this.LbSaleMan.Name = "LbSaleMan";
            this.LbSaleMan.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbSaleMan.SizeF = new System.Drawing.SizeF(490.6667F, 23.00001F);
            this.LbSaleMan.StylePriority.UseBorderDashStyle = false;
            this.LbSaleMan.StylePriority.UseBorders = false;
            this.LbSaleMan.StylePriority.UseFont = false;
            this.LbSaleMan.StylePriority.UseTextAlignment = false;
            this.LbSaleMan.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbNOte
            // 
            this.LbNOte.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbNOte.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbNOte.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbNOte.LocationFloat = new DevExpress.Utils.PointFloat(23.91663F, 259.5833F);
            this.LbNOte.Multiline = true;
            this.LbNOte.Name = "LbNOte";
            this.LbNOte.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbNOte.SizeF = new System.Drawing.SizeF(490.6667F, 23.00001F);
            this.LbNOte.StylePriority.UseBorderDashStyle = false;
            this.LbNOte.StylePriority.UseBorders = false;
            this.LbNOte.StylePriority.UseFont = false;
            this.LbNOte.StylePriority.UseTextAlignment = false;
            this.LbNOte.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // LbNetTot
            // 
            this.LbNetTot.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.LbNetTot.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.LbNetTot.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.LbNetTot.LocationFloat = new DevExpress.Utils.PointFloat(344.5417F, 298.125F);
            this.LbNetTot.Multiline = true;
            this.LbNetTot.Name = "LbNetTot";
            this.LbNetTot.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbNetTot.SizeF = new System.Drawing.SizeF(170.0416F, 23F);
            this.LbNetTot.StylePriority.UseBorderDashStyle = false;
            this.LbNetTot.StylePriority.UseBorders = false;
            this.LbNetTot.StylePriority.UseFont = false;
            this.LbNetTot.StylePriority.UseTextAlignment = false;
            this.LbNetTot.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.LbNetTot.TextFormatString = "{0:n3}";
            // 
            // LbReporttitle
            // 
            this.LbReporttitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(209)))), ((int)(((byte)(107)))));
            this.LbReporttitle.BorderColor = System.Drawing.Color.Black;
            this.LbReporttitle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.LbReporttitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.LbReporttitle.LocationFloat = new DevExpress.Utils.PointFloat(131.6658F, 48.514F);
            this.LbReporttitle.Multiline = true;
            this.LbReporttitle.Name = "LbReporttitle";
            this.LbReporttitle.Scripts.OnTextChanged = "LbCoName_TextChanged";
            this.LbReporttitle.SizeF = new System.Drawing.SizeF(393.9037F, 33.51398F);
            this.LbReporttitle.StyleName = "Title";
            this.LbReporttitle.StylePriority.UseBackColor = false;
            this.LbReporttitle.StylePriority.UseBorderColor = false;
            this.LbReporttitle.StylePriority.UseFont = false;
            this.LbReporttitle.StylePriority.UseForeColor = false;
            this.LbReporttitle.StylePriority.UseTextAlignment = false;
            this.LbReporttitle.Text = "سند قبض نقدي";
            this.LbReporttitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // LbCoName
            // 
            this.LbCoName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(209)))), ((int)(((byte)(107)))));
            this.LbCoName.BorderColor = System.Drawing.Color.Black;
            this.LbCoName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.LbCoName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.LbCoName.LocationFloat = new DevExpress.Utils.PointFloat(36.41666F, 10.00001F);
            this.LbCoName.Name = "LbCoName";
            this.LbCoName.Scripts.OnTextChanged = "LbCoName_TextChanged";
            this.LbCoName.SizeF = new System.Drawing.SizeF(559.9584F, 33.51399F);
            this.LbCoName.StyleName = "Title";
            this.LbCoName.StylePriority.UseBackColor = false;
            this.LbCoName.StylePriority.UseBorderColor = false;
            this.LbCoName.StylePriority.UseFont = false;
            this.LbCoName.StylePriority.UseForeColor = false;
            this.LbCoName.StylePriority.UseTextAlignment = false;
            this.LbCoName.Text = "CO Name";
            this.LbCoName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // Detail
            // 
            this.Detail.HeightF = 9.166654F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Acc.ViewModels.TransactionFixedVM);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Arial", 14.25F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.Title.Name = "Title";
            // 
            // DetailCaption2
            // 
            this.DetailCaption2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(229)))), ((int)(((byte)(250)))));
            this.DetailCaption2.BorderColor = System.Drawing.Color.White;
            this.DetailCaption2.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailCaption2.BorderWidth = 2F;
            this.DetailCaption2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.DetailCaption2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.DetailCaption2.Name = "DetailCaption2";
            this.DetailCaption2.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData2
            // 
            this.DetailData2.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData2.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData2.BorderWidth = 2F;
            this.DetailData2.Font = new System.Drawing.Font("Arial", 8.25F);
            this.DetailData2.ForeColor = System.Drawing.Color.Black;
            this.DetailData2.Name = "DetailData2";
            this.DetailData2.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailData3_Odd.BorderWidth = 1F;
            this.DetailData3_Odd.Font = new System.Drawing.Font("Arial", 8.25F);
            this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
            this.DetailData3_Odd.Name = "DetailData3_Odd";
            this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageInfo
            // 
            this.PageInfo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // LbTafqet
            // 
            this.LbTafqet.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.LbTafqet.LocationFloat = new DevExpress.Utils.PointFloat(23.91663F, 298.125F);
            this.LbTafqet.Multiline = true;
            this.LbTafqet.Name = "LbTafqet";
            this.LbTafqet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LbTafqet.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.LbTafqet.SizeF = new System.Drawing.SizeF(310.2084F, 23F);
            this.LbTafqet.StylePriority.UseBorders = false;
            this.LbTafqet.Text = "LbTafqet";
            // 
            // XtraReceiptVoucherCashWithoutCost
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.Detail});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 8, 100);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption2,
            this.DetailData2,
            this.DetailData3_Odd,
            this.PageInfo});
            this.StyleSheetPath = "";
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel LbPrintTime;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel LbReporttitle;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle Title;
        private DevExpress.XtraReports.UI.XRControlStyle DetailCaption2;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData2;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData3_Odd;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
        public DevExpress.XtraReports.UI.XRLabel LbAccFromPaidName;
        public DevExpress.XtraReports.UI.XRLabel LbVoucherNo;
        public DevExpress.XtraReports.UI.XRLabel LbVDate;
        public DevExpress.XtraReports.UI.XRLabel LbAccPaidFrom;
        public DevExpress.XtraReports.UI.XRLabel LbDebitAccNO;
        public DevExpress.XtraReports.UI.XRLabel LbAccDebitName;
        public DevExpress.XtraReports.UI.XRLabel LbSaleMan;
        public DevExpress.XtraReports.UI.XRLabel LbNOte;
        public DevExpress.XtraReports.UI.XRLabel LbNetTot;
        public DevExpress.XtraReports.UI.XRLabel LbCoName;
        public DevExpress.XtraReports.UI.XRLabel LbTafqet;
    }
}
