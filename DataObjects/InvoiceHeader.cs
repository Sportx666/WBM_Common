using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class InvoiceHeader : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListInvoiceNumber { get; set; }
            public List<string> ListInvoicePrefix { get; set; }

            public DateTime? InvoiceDateTime_from { get; set; }

            public DateTime? InvoiceDateTime_to { get; set; }
            public List<int> ListInvoiceCycleID { get; set; }
            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }
            public List<int> ListInvoiceFormatID { get; set; }

            public Decimal? ClientExGSTAmount_min { get; set; }

            public Decimal? ClientExGSTAmount_max { get; set; }

            public Decimal? ClientGSTAmount_min { get; set; }

            public Decimal? ClientGSTAmount_max { get; set; }

            public Decimal? ClientTotalAmount_min { get; set; }

            public Decimal? ClientTotalAmount_max { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2
            }

            public SearchModeType SearchMode { get; set; } = SearchModeType.Undefined;

            public System.Boolean IsValid()
            {
                ValidationErrors = new List<Tuple<string, string>>();

                System.Boolean rtnVal = true; // assume valid
                if (SearchMode == SearchModeType.AllRecords)
                {
                    // all records - no mandatory fields
                }
                else if (SearchMode == SearchModeType.SingleID)
                {
                    if (ID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ID", "Single ID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

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
                sb.Append(@"SELECT InvoiceHeader.*,
                        Company.ShortName AS Company_ShortName,
                        Owner.Name AS Owner_Name,
                        Site.PublicID AS Site_PublicID,
                        InvoiceCycle.PublicID AS InvoiceCycle_PublicID,
                        Security_User.PublicID as Created_UserPublicID
                        FROM InvoiceHeader
                    LEFT JOIN Company ON InvoiceHeader.CompanyID = Company.ID
                    LEFT JOIN Owner ON InvoiceHeader.OwnerID = Owner.ID
                    LEFT JOIN Site ON InvoiceHeader.SiteID = Site.ID
                    LEFT JOIN InvoiceCycle ON InvoiceHeader.InvoiceCycleID = InvoiceCycle.ID
                    LEFT JOIN Security_User on InvoiceHeader.CreatedUserID = Security_User.ID
                    WHERE 1=1 ");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (BaseGenerateSqlQuery("InvoiceHeader", out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListInvoiceCycleID(ListInvoiceCycleID, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListInvoiceFormatID(ListInvoiceFormatID, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListInvoiceNumber(ListInvoiceNumber, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListInvoicePrefix(ListInvoicePrefix, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("ClientExGSTAmount", ClientGSTAmount_min, ClientExGSTAmount_max, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("ClientGSTAmount", ClientGSTAmount_min, ClientGSTAmount_max, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("ClientTotalAmount", ClientTotalAmount_min, ClientTotalAmount_max, out generateSQL, out generateListParamater, "InvoiceHeader."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("InvoiceDate", InvoiceDateTime_from, InvoiceDateTime_to, out generateSQL, out generateListParamater, "InvoiceHeader."))
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
            [Range(1, int.MaxValue, ErrorMessage = "InvoiceNumber is required")]
            public int InvoiceNumber { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "InvoicePrefix is required")]
            public string InvoicePrefix { get; set; }
            [Required(ErrorMessage = "InvoiceDate is required")]
            public DateTime InvoiceDate { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "InvoiceCycleID is required")]
            public int InvoiceCycleID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ClientExGSTAmount is required")]
            public decimal ClientExGSTAmount { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ClientGSTAmount is required")]
            public decimal ClientGSTAmount { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ClientTotalAmount is required")]
            public decimal ClientTotalAmount { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "InvoiceFormatID is required")]
            public int InvoiceFormatID { get; set; }
            public DateTime? FirstExportDateTime { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? InvoiceCycle_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "InvoiceNumber", "InvoicePrefix", "InvoiceDate", "InvoiceCycleID", "CompanyID"
        , "SiteID", "OwnerID", "ClientExGSTAmount", "ClientGSTAmount", "ClientTotalAmount", "InvoiceFormatID" };

        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("InvoiceNumber", "Invoice Number"),
                        new ChangeTrackerData("InvoicePrefix", "Invoice Prefix"),
                        new ChangeTrackerData("InvoiceDate", "Invoice Date"),
                        new ChangeTrackerData("InvoiceCycleID", "Invoice Cycle ID"),
                        new ChangeTrackerData("CompanyID", "Company ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("ClientExGSTAmount", "Client ExGST Amount"),
                        new ChangeTrackerData("ClientGSTAmount", "Client GST Amount"),
                        new ChangeTrackerData("ClientTotalAmount", "Client Total Amount"),
                        new ChangeTrackerData("InvoiceFormatID", "Invoice Format ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}