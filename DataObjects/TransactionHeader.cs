using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class TransactionHeader : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }

            public List<int> ListOwnerID { get; set; }

            public List<int> ListSiteID { get; set; }

            public List<int> ListTransactionSourceID { get; set; }

            public DateTime? TransactionDateTime_from { get; set; }

            public DateTime? TransactionDateTime_to { get; set; }

            public List<int> TransactionTypeID { get; set; }

            public string TransactionReference { get; set; }

            public string Description { get; set; }

            public int InvoiceID { get; set; }

            public DateTime? InvoiceDateTime_from { get; set; }

            public DateTime? InvoiceDateTime_to { get; set; }

            public Decimal? InvoiceExFuel_min { get; set; }

            public Decimal? InvoiceExFuel_max { get; set; }

            public Boolean? ExceptionFlag { get; set; }

            public Boolean? CanInvoice { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                InvoiceID = 2
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
                else if (SearchMode == SearchModeType.InvoiceID)
                {
                    if (InvoiceID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("InvoiceID", "InvoiceID must be specified."));
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

                    if (!DataObjectValidator.DateMinMax("Invoice Date", InvoiceDateTime_from, InvoiceDateTime_to, out ListLocalValidationErrors))
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (!DataObjectValidator.DecimalMinMax("Invoice Ex Fuel", InvoiceExFuel_min, InvoiceExFuel_max, out ListLocalValidationErrors))
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
                sb.Append(@"SELECT TransactionHeader.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            TransactionSource.PublicID AS TransactionSource_PublicID,
                            TransactionExceptionType.PublicID AS TransactionType_PublicID,
                            InvoiceCycle.Description AS TransactionHeader_InvoiceNumber,
                            Security_User.PublicID as Created_UserPublicID
                    FROM TransactionHeader 
                        LEFT JOIN Site ON TransactionHeader.SiteID = Site.ID
                        LEFT JOIN Company ON TransactionHeader.CompanyID = Company.ID
                        LEFT JOIN Owner ON TransactionHeader.OwnerID = Owner.ID
                        LEFT JOIN TransactionSource ON TransactionHeader.TransactionSourceID = TransactionSource.ID
                        LEFT JOIN TransactionExceptionType ON TransactionHeader.TransactionTypeID = TransactionExceptionType.ID
                        LEFT JOIN InvoiceHeader ON TransactionHeader.InvoiceHeaderID = InvoiceHeader.ID
                        LEFT JOIN Security_User on TransactionHeader.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListTransactionSourceID(ListTransactionSourceID, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("TransactionDateTime", TransactionDateTime_from, TransactionDateTime_to, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListTransactionTypeID(TransactionTypeID, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(TransactionReference))
                {
                    sb.Append(" AND TransactionHeader.TransactionReference like '%' + @TransactionReference + '%' ");
                    ListSqlParameter.Add(new SqlParameter("@TransactionReference", TransactionReference));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND TransactionHeader.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (DataObjectSQLGenerator.Date_FromTo("Invoice Date", InvoiceDateTime_from, InvoiceDateTime_to, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("Invoice Ex Fuel", InvoiceExFuel_min, InvoiceExFuel_max, out generateSQL, out generateListParamater, "TransactionHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }   

                if (ExceptionFlag.HasValue)
                {
                    sb.Append(" AND TransactionHeader.ExceptionFlag = @ExceptionFlag");
                    ListSqlParameter.Add(new SqlParameter("@ExceptionFlag", ExceptionFlag.Value));
                }

                if (CanInvoice.HasValue)
                {
                    sb.Append(" AND TransactionHeader.CanInvoice = @CanInvoice");
                    ListSqlParameter.Add(new SqlParameter("@CanInvoice", CanInvoice.Value));
                }
                if (BaseGenerateSqlQuery("TransactionHeader", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "TransactionSourceID is required")]
            public int TransactionSourceID { get; set; }
            [Required(ErrorMessage = "TransactionDateTime is required")]
            public DateTime TransactionDateTime { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "TransactionTypeID is required")]
            public int TransactionTypeID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "TransactionReference is required")]
            public string TransactionReference { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "InvoiceHeaderID is required")]
            public int InvoiceHeaderID { get; set; }

            public DateTime? InvoiceDateTime { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "InvoiceExFuel is required")]
            public Decimal InvoiceExFuel { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "InvoiceFuel is required")]
            public Decimal InvoiceFuel { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "InvoiceGST is required")]
            public Decimal InvoiceGST { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "InvoiceEXGST is required")]
            public Decimal InvoiceExGST { get; set; }
            [Required(ErrorMessage = "ExceptionFlag is required")]
            public Boolean ExceptionFlag { get; set; }
            [Required(ErrorMessage = "CanInvoice is required")]
            public Boolean CanInvoice { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? TransactionSource_PublicID { get; set; }
            public string? TransactionType_PublicID { get; set; }
            public string? InvoiceHeader_InvoiceNumber { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "OwnerID", "SiteID", "TransactionSourceID", "TransactionDateTime", "TransactionTypeID"
                , "TransactionReference" , "Description" , "InvoiceHeaderID" , "InvoiceExFuel", "InvoiceFuel", "InvoiceGST", "InvoiceExGST", "ExceptionFlag", "CanInvoice" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("CompanyID", "Company ID"),
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("TransactionSourceID", "Transaction Source ID"),
                        new ChangeTrackerData("TransactionDateTime", "Transaction DateTime"),
                        new ChangeTrackerData("TransactionTypeID", "Transaction Type ID"),
                        new ChangeTrackerData("TransactionReference", "Transaction Reference"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("InvoiceDateTime", "Invoice DateTime"),
                        new ChangeTrackerData("InvoiceExFuel", "Invoice ExFuel", true),
                        new ChangeTrackerData("InvoiceFuel", "Invoice Fuel", true),
                        new ChangeTrackerData("InvoiceGST", "Invoice GST", true),
                        new ChangeTrackerData("InvoiceExGST", "Invoice ExGST", true),
                        new ChangeTrackerData("ExceptionFlag", "Exception Flag"),
                        new ChangeTrackerData("CanInvoice", "Can Invoice")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
