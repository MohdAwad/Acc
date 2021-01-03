using System.ComponentModel.DataAnnotations;

namespace Acc.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]

        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resource))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Id { get; set; }
        public int UserType { get; set; }
        public int fCompanyId { get; set; }
        public int AccountStatus { get; set; }
        public string EmployeeID { get; set; }


        public string RealPass { get; set; }
        public string UserId { get; set; }







        //------------Per---------------//


        //[Display(Name = "ShowChartOfAccount", ResourceType = typeof(Resources.Resource))]

        //public bool ShowChartOfAccount { get; set; }

        //[Display(Name = "AddAccount", ResourceType = typeof(Resources.Resource))]

        //public bool AddAccount { get; set; }

        //[Display(Name = "DeleteAccount", ResourceType = typeof(Resources.Resource))]
        //public bool DeleteAccount { get; set; }

        //[Display(Name = "UpdateAccountName", ResourceType = typeof(Resources.Resource))]

        //public bool UpdateAccountName { get; set; }

        //[Display(Name = "UpdateAccountKid", ResourceType = typeof(Resources.Resource))]

        //public bool UpdateAccountKid { get; set; }

        //[Display(Name = "UpdateAcountType", ResourceType = typeof(Resources.Resource))]

        //public bool UpdateAcountType { get; set; }

        //[Display(Name = "StopAccount", ResourceType = typeof(Resources.Resource))]

        //public bool StopAccount { get; set; }

        //----CompanySetup--//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCompany { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddCompany { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateCompany { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteCompany { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintCompany { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowUser { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddUser { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateUser { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteUser { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintUser { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCurrancy { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddCurrancy { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateCurrancy { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteCurrancy { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintCurrancy { get; set; }

        //-----//



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCurrancyValue { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddCurrancyValue { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintCurrancyValue { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSalesman { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSalesman { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSalesman { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSalesman { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSalesman { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTransaction { get; set; }//n3n3

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddTransaction { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateTransaction { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteTransaction { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintTransaction { get; set; }

        //-----//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTransActionTrans { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddTransActionTrans { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteTransActionTrans { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyTransActionTrans { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateTransActionTrans { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintTransActionTrans { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportTransActionTrans { get; set; }

        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportTransActionTrans { get; set; }

        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachTransActionTrans { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowServicegroup { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddServicegroup { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateServicegroup { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteServicegroup { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintServicegroup { get; set; }

        //-----//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowBankswithdrawnfrom { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddBankswithdrawnfrom { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateBankswithdrawnfrom { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintBankswithdrawnfrom { get; set; }

        //-----//
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowBank { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddBank { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateBank { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteBank { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintBank { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowOtheraccount { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddOtheraccount { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateOtheraccount { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteOtheraccount { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintOtheraccount { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowManagement { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddManagement { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateManagement { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteManagement { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintManagement { get; set; }

        //-----//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowDepartment { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddDepartment { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateDepartment { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteDepartment { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintDepartment { get; set; }

        //-----//



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSections { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSections { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSections { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSections { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSections { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowLocations { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddLocations { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateLocations { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteLocations { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintLocations { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTrustee { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddTrustee { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateTrustee { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteTrustee { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintTrustee { get; set; }

        //-----//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSupplierbanks { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSupplierbanks { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSupplierbanks { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSupplierbanks { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSupplierbanks { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSuppliercountries { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSuppliercountries { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSuppliercountries { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSuppliercountries { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSuppliercountries { get; set; }

        //-----//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSupplierbankcountries { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSupplierbankcountries { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSupplierbankcountries { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSupplierbankcountries { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSupplierbankcountries { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSupplierbankbranches { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSupplierbankbranches { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSupplierbankbranches { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSupplierbankbranches { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSupplierbankbranches { get; set; }

        //-----//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSuppliercity { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSuppliercity { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSuppliercity { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSuppliercity { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSuppliercity { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSuppliersbankcity { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSuppliersbankcity { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSuppliersbankcity { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSuppliersbankcity { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintSuppliersbankcity { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCustomercity { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddCustomercity { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateCustomercity { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteCustomercity { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintCustomercity { get; set; }

        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCustomerarea { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddCustomerarea { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateCustomerarea { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteCustomerarea { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintCustomerarea { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowService { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddService { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateService { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteService { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintService { get; set; }



       


        //--------------Operation---------//
        //-----//

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTransferbetweenaccounts { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool TransferTransferbetweenaccounts { get; set; }
        //--//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowFreezunfreezmonthtransaction { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool FreezFreezunfreezmonthtransaction { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool unFreezFreezunfreezmonthtransaction { get; set; }
        //-------/

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowEstimatedbudget { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddEstimatedbudget { get; set; }
        //------------End Operation-----------//



        ////-----Account-----------//


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowChartofaccount { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddmainaccountChartofaccount { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateChartofaccoun { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteChartofaccoun { get; set; }






        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddsubaccountChartofaccount { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdatesubaccountChartofaccount { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeletesubaccountChartofaccount { get; set; }





        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowChartofcostcenter { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddmainaccountChartofcostcenter { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateChartofcostcenter { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteChartofcostcenter { get; set; }





        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddsubChartofcostcenter { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdatesubChartofcostcenter { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeletesubChartofcostcenter { get; set; }



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool Showcustomeraccount { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool Addcustomeraccount { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool Updatecustomeraccount { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool Deletecustomeraccount { get; set; }





        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSupplieraccount { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddSupplieraccount { get; set; }


        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateSupplieraccount { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteSupplieraccount { get; set; }
        //End Account-----------/



        //---Asset--///////


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowAssetType { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddAssetType { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateAssetType { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteAssetType { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintAssetType { get; set; }




        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCalculationOfConsumption { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddCalculationOfConsumption { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowSaleAsset { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddShowSaleAsset { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowAsset { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddAsset { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateAsset { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteAsset { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintAsset { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowAssetToAcc { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddAssetToAcc { get; set; }




        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowDeliveryAssetTrustee { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddDeliveryAssetTrustee { get; set; }



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowConsumAsset { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddConsumAsset { get; set; }




        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowDefinitionAsset { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddDefinitionAsset { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowAssetMaintenance { get; set; }

        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddAssetMaintenance { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateAssetMaintenance { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteAssetMaintenance { get; set; }




        //--End Asset---/
        //---Report
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepAccountStatement { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepTrialBalance { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepAccountingDetailsReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepChequesReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepCustSuppBalances { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepTrialBalanceYearly { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepTransActionDetails { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepTrankingChequesReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepSalesReport { get; set; }
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepExpenseAnalysisDetailReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepSearchForTrans { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepPaymentChequesReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepServiceReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepCustomerBalancesReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepPaymTrankingChequesReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepServiceTaxReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepSupplierBalancesReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepExpenseAnalysisReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepProfitlossreport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepTempPrepaidReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepPivotReportAccounts { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepAssetsReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepTempRevenueReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepAssetDepreciationReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepEstimatedBudgetForAccount { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepConsumptionByTypeReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepEstimatedBudgetForAccountAll { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepDefinitionAssetSiteReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepEstimatedBudgetForAccountYear { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepSoldAsset { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepEstimatedBudgetForCostCenters { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepConsumAssetReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepEstimatedBudgetCostAllMonth { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepMaintenanceAssetReport { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepEstimatedBudgetForCostCenterYear { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool RepAssetsTransferMovements { get; set; }

        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepAccountStatement { get; set; }


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepTrialBalance { get; set; }


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepAccountingDetailsPrintReport { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepChequesPrintReport { get; set; }


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepCustSuppBalances { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepTrialBalanceYearly { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepTransActionDetails { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepTrankingChequesPrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepSalesPrintReport { get; set; }





        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepExpenseAnalysisDetailPrintReport { get; set; }





        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepSearchForTrans { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepPaymentChequesPrintReport { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepServicePrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepCustomerBalancesPrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepPaymTrankingChequesPrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepServiceTaxPrintReport { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepSupplierBalancesPrintReport { get; set; }






        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepExpenseAnalysisPrintReport { get; set; }






        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepProfitlossPrintReport { get; set; }






        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepTempPPrintRepaidPrintReport { get; set; }



        /// <summary>


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepPivotPrintReportAccounts { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepAssetsPrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepTempRevenuePrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepAssetDepreciationPrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepEstimatedBudgetForAccount { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepConsumptionByTypePrintReport { get; set; }

        /// <summary>


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepEstimatedBudgetForAccountAll { get; set; }


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepDefinitionAssetSitePrintReport { get; set; }


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepEstimatedBudgetForAccountYear { get; set; }


        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepSoldAsset { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepEstimatedBudgetForCostCenters { get; set; }

        /// <summary>



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepConsumAssetPrintReport { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepEstimatedBudgetCostAllMonth { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepMaintenanceAssetPrintReport { get; set; }




        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepEstimatedBudgetForCostCenterYear { get; set; }



        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintRepAssetsTransferMovements { get; set; }



        //-----End Report----------//





        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReceiptVoucherBank { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddReceiptVoucherBank { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteReceiptVoucherBank { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyReceiptVoucherBank { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateReceiptVoucherBank { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintReceiptVoucherBank { get; set; }
        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportReceiptVoucherBank { get; set; }
        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportReceiptVoucherBank { get; set; }

        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachReceiptVoucherBank { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTempPrepaid { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddTempPrepaid { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteTempPrepaid { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyTempPrepaid { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateTempPrepaid { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintTempPrepaid { get; set; }


        [Display(Name = "Collect", ResourceType = typeof(Resources.Resource))]
        public bool AccumulativeTempPrepaid { get; set; }

        
        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachTempPrepaid { get; set; }



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowDebitNote { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddDebitNote { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteDebitNote { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyDebitNote { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateDebitNote { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintDebitNote { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportDebitNote { get; set; }


        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportDebitNote { get; set; }



        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachDebitNote { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowServiceBill { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddServiceBill { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteServiceBill { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyServiceBill { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateServiceBill { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintServiceBill { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportServiceBill { get; set; }


        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportServiceBill { get; set; }



        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachServiceBill { get; set; }


     

        

        //[Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        //public bool ShowReceiptVoucherBankReceivedCheque { get; set; }
        //[Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        //public bool AddReceiptVoucherBankReceivedCheque { get; set; }
        //[Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        //public bool DeleteReceiptVoucherBankReceivedCheque { get; set; }
        //[Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        //public bool CopyReceiptVoucherBankReceivedCheque { get; set; }
        //[Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        //public bool UpdateReceiptVoucherBankReceivedCheque { get; set; }
        //[Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        //public bool PrintReceiptVoucherBankReceivedCheque { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowOpeningBalance { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddOpeningBalance { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteOpeningBalance { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyOpeningBalance { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateOpeningBalance { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintOpeningBalance { get; set; }



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReceiptVoucherCash { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddReceiptVoucherCash { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteReceiptVoucherCash { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyReceiptVoucherCash { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateReceiptVoucherCash { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintReceiptVoucherCash { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportReceiptVoucherCash { get; set; }

        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportReceiptVoucherCash { get; set; }

        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachReceiptVoucherCash { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTempRevenueReceived { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddTempRevenueReceived { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteTempRevenueReceived { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyTempRevenueReceived { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateTempRevenueReceived { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintTempRevenueReceived { get; set; }

        [Display(Name = "Collect", ResourceType = typeof(Resources.Resource))]
        public bool AccumulativeTempRevenueReceived { get; set; }

        
        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachTempRevenueReceived { get; set; }





        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCreditNote { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddCreditNote { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteCreditNote { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyCreditNote { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateCreditNote { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintCreditNote { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportCreditNote { get; set; }

        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportCreditNote { get; set; }

        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachCreditNote { get; set; }

        //[Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        //public bool ShowPaymentVoucherBankPaymentCheque { get; set; }
        //[Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        //public bool AddPaymentVoucherBankPaymentCheque { get; set; }
        //[Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        //public bool DeletePaymentVoucherBankPaymentCheque { get; set; }
        //[Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        //public bool CopyPaymentVoucherBankPaymentCheque { get; set; }
        //[Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        //public bool UpdatePaymentVoucherBankPaymentCheque { get; set; }
        //[Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        //public bool PrintPaymentVoucherBankPaymentCheque { get; set; }



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowPaymentVoucherBank { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddPaymentVoucherBank { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeletePaymentVoucherBank { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyPaymentVoucherBank { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdatePaymentVoucherBank { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintPaymentVoucherBank { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportPaymentVoucherBank { get; set; }


        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportPaymentVoucherBank { get; set; }


        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachPaymentVoucherBank { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowPaymentVoucherCash { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddPaymentVoucherCash { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeletePaymentVoucherCash { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyPaymentVoucherCash { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdatePaymentVoucherCash { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintPaymentVoucherCash { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportPaymentVoucherCash { get; set; }


        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportPaymentVoucherCash { get; set; }

        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachPaymentVoucherCash { get; set; }



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowDepositInTheBank { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddDepositInTheBank { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteDepositInTheBank { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyDepositInTheBank { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateDepositInTheBank { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintDepositInTheBank { get; set; }


        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportDepositInTheBank { get; set; }


        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportDepositInTheBank { get; set; }


        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachDepositInTheBank { get; set; }

        
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReturnACheque { get; set; }
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTransferFromFundUC { get; set; }
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowDrawingUC { get; set; }
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowPaymentchequeUC { get; set; }
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTransferFromFundCD { get; set; }
        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReturnAChequeCD { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowTransferFromFundToChequeE { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowPaymentChequeEndorsement { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReturnChequeUnderCollection { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReturnChequeEndorsement { get; set; }



        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowFundChequesDrawnFromUC { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReturnedChequeFund { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowCourtFund { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowPostdatedCheques { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReturnPostdatedCheques { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReturnPaidPostdatedCheques { get; set; }




        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowReceiptVoucherCashMultiAccount { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddReceiptVoucherCashMultiAccount { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeleteReceiptVoucherCashMultiAccount { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyReceiptVoucherCashMultiAccount { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdateReceiptVoucherCashMultiAccount { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintReceiptVoucherCashMultiAccount { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportReceiptVoucherCashMultiAccount { get; set; }

        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportReceiptVoucherCashMultiAccount { get; set; }

        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachReceiptVoucherCashMultiAccount { get; set; }


        [Display(Name = "Show", ResourceType = typeof(Resources.Resource))]
        public bool ShowPaymentVoucherCashMultiAccount { get; set; }
        [Display(Name = "Add", ResourceType = typeof(Resources.Resource))]
        public bool AddPaymentVoucherCashMultiAccount { get; set; }
        [Display(Name = "Delete", ResourceType = typeof(Resources.Resource))]
        public bool DeletePaymentVoucherCashMultiAccount { get; set; }
        [Display(Name = "Copy", ResourceType = typeof(Resources.Resource))]
        public bool CopyPaymentVoucherCashMultiAccount { get; set; }
        [Display(Name = "Update", ResourceType = typeof(Resources.Resource))]
        public bool UpdatePaymentVoucherCashMultiAccount { get; set; }
        [Display(Name = "Print", ResourceType = typeof(Resources.Resource))]
        public bool PrintPaymentVoucherCashMultiAccount { get; set; }

        [Display(Name = "Export", ResourceType = typeof(Resources.Resource))]
        public bool ExportPaymentVoucherCashMultiAccount { get; set; }

        [Display(Name = "UnExport", ResourceType = typeof(Resources.Resource))]
        public bool UnExportPaymentVoucherCashMultiAccount { get; set; }

        [Display(Name = "Attach", ResourceType = typeof(Resources.Resource))]
        public bool AttachPaymentVoucherCashMultiAccount { get; set; }




    }
}