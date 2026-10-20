using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class TransactionDetail : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListTransactionHeaderID { get; set; }

            public List<int> ListCompanyID { get; set; }

            public List<int> ListOwnerID { get; set; }

            public List<int> ListSiteID { get; set; }

            public DateTime? TransactionDateTime_from { get; set; }

            public DateTime? TransactionDateTime_to { get; set; }

            public List<int> ListRateCodeID { get; set; }

            public string RateCode { get; set; }

            public string ChargeDescription { get; set; }

            public string FullDescription { get; set; }

            public int Quantity { get; set; }

            public Boolean? Override { get; set; }

            public Decimal? ActualCustomerExtended_min { get; set; }

            public Decimal? ActualCustomerExtended_max { get; set; }

            public Boolean? ExceptionFlag { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                TransactionHeaderID = 2
            }

            public SearchModeType SearchMode { get; set; } = SearchModeType.Undefined;

            public System.Boolean IsValid()
            {
                ValidationErrors = new List<Tuple<string, string>>();

                System.Boolean rtnVal = true; // assume valid

                if (SearchMode == SearchModeType.SingleID)
                {
                    if (ID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ID", "Single ID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.TransactionHeaderID)
                {
                    if (ListTransactionHeaderID == null || ListTransactionHeaderID.Count < 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("TransactionHeader", "TransactionHeaderID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

                    if (!DataObjectValidator.DateMinMax("Transaction Date", TransactionDateTime_from, TransactionDateTime_to, out ListLocalValidationErrors))
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (!DataObjectValidator.DecimalMinMax("Actual Customer Extended", ActualCustomerExtended_min, ActualCustomerExtended_max, out ListLocalValidationErrors))
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (!BaseIsValid(out ListLocalValidationErrors))
                    {
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }
                }

                if (ValidationErrors.Count > 0)
                {
                    rtnVal = false;
                }

                return rtnVal;
            }

            public void GenerateSqlQuery()
            {
                ListSqlParameter = new List<SqlParameter>();

                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT TransactionDetail.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            TransactionHeader.Description AS TransactionHeader_Description,
                            Security_User.PublicID as Created_UserPublicID
                    FROM TransactionDetail 
                        LEFT JOIN Site ON TransactionDetail.SiteID = Site.ID
                        LEFT JOIN Company ON TransactionDetail.CompanyID = Company.ID
                        LEFT JOIN Owner ON TransactionDetail.OwnerID = Owner.ID
                        LEFT JOIN TransactionHeader ON TransactionDetail.TransactionHeaderID = TransactionHeader.ID
                        LEFT JOIN Security_User on TransactionDetail.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListTransactionHeaderID(ListTransactionHeaderID, out generateSQL, out generateListParamater, "TransactionDetail."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "TransactionDetail."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "TransactionDetail."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "TransactionDetail."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("TransactionDateTime", TransactionDateTime_from, TransactionDateTime_to, out generateSQL, out generateListParamater, "TransactionDetail."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListRateCodeID(ListRateCodeID, out generateSQL, out generateListParamater, "TransactionDetail."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(RateCode))
                {
                    sb.Append(" AND TransactionDetail.RateCode like '%' + @RateCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@RateCode", RateCode));
                }

                if (!string.IsNullOrEmpty(ChargeDescription))
                {
                    sb.Append(" AND TransactionDetail.ChargeDescription like '%' + @ChargeDescription + '%'");
                    ListSqlParameter.Add(new SqlParameter("@ChargeDescription", ChargeDescription));
                }

                if (!string.IsNullOrEmpty(FullDescription))
                {
                    sb.Append(" AND TransactionDetail.FullDescription like '%' + @FullDescription + '%'");
                    ListSqlParameter.Add(new SqlParameter("@FullDescription", FullDescription));
                }

                if (Quantity > 0)
                {
                    sb.Append(" AND TransactionDetail.Quantity = @Quantity");
                    ListSqlParameter.Add(new SqlParameter("@Quantity", Quantity));
                }

                if (Override.HasValue)
                {
                    sb.Append(" AND TransactionDetail.Override = @Override");
                    ListSqlParameter.Add(new SqlParameter("@Override", Override.Value));
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("ActualCustomerExtended", ActualCustomerExtended_min, ActualCustomerExtended_max, out generateSQL, out generateListParamater, "TransactionDetail."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (ExceptionFlag.HasValue)
                {
                    sb.Append(" AND TransactionDetail.ExceptionFlag = @ExceptionFlag");
                    ListSqlParameter.Add(new SqlParameter("@ExceptionFlag", ExceptionFlag.Value));
                }

                if (BaseGenerateSqlQuery("TransactionDetail", out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                SqlQuery = sb.ToString();
            }


        }
        public dbRow dbRow_instance { get; set; }
        public class dbRow : dbRowCreatedLast_Base
        {
            [Range(-int.MaxValue, int.MaxValue, ErrorMessage = "TransactionHeaderID is required")]
            public int TransactionHeaderID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Required(ErrorMessage = "TransactionDateTime is required")]
            public DateTime TransactionDateTime { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "RateCodeID is required")]
            public int RateCodeID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "RateCode is required")]
            public string RateCode { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "ChargeDescription is required")]
            public string ChargeDescription { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "FullDescription is required")]
            public string FullDescription { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Quantity is required")]
            public int Quantity { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "CalculatedRate is required")]
            public Decimal CalculatedRate { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "CalculatedExtended is required")]
            public Decimal CalculatedExtended { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "CalculatedFuel is required")]
            public Decimal CalculatedFuel { get; set; }
            [Required(ErrorMessage = "Override is required")]
            public Boolean Override { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ActualCustomerRate is required")]
            public decimal ActualCustomerRate { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ActualCustomerExtended is required")]
            public decimal ActualCustomerExtended { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ActualCustomerFuel is required")]
            public decimal ActualCustomerFuel { get; set; }
            [Required(ErrorMessage = "ExceptionFlag is required")]

            [NotMapped]
            public Decimal ActualCustomerExGST
            {
                get
                {
                    return ActualCustomerExtended + ActualCustomerFuel;
                }
            }
            public Boolean ExceptionFlag { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? TransactionHeader_Description { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "OwnerID", "SiteID", "TransactionHeaderID", "TransactionDateTime", "RateCodeID"
                , "RateCode" , "ChargeDescription" , "FullDescription" , "Quantity" , "CalculatedRate", "CalculatedExtended", "CalculatedFuel", "Override", "ActualCustomerRate",
            "ActualCustomerExtended", "ActualCustomerFuel", "ExceptionFlag" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("TransactionHeaderID", "Transaction Header ID"),
                        new ChangeTrackerData("CompanyID", "Company ID"),
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("TransactionDateTime", "Transaction DateTime"),
                        new ChangeTrackerData("RateCodeID", "Rate Code ID"),
                        new ChangeTrackerData("RateCode", "Rate Code"),
                        new ChangeTrackerData("ChargeDescription", "Charge Description"),
                        new ChangeTrackerData("FullDescription", "Full Description"),
                        new ChangeTrackerData("Quantity", "Quantity"),
                        new ChangeTrackerData("CalculatedRate", "Calculated Rate", true),
                        new ChangeTrackerData("CalculatedExtended", "Calculated Extended", true),
                        new ChangeTrackerData("CalculatedFuel", "Calculated Fuel", true),
                        new ChangeTrackerData("Override", "Override"),
                        new ChangeTrackerData("ActualCustomerRate", "Actual Customer Rate", true),
                        new ChangeTrackerData("ActualCustomerExtended", "Actual Customer Extended", true),
                        new ChangeTrackerData("ActualCustomerFuel", "Actual Customer Fuel", true),
                        new ChangeTrackerData("ExceptionFlag", "Exception Flag")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
