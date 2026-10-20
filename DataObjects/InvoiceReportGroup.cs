using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace wbm_common.DataObjects
{
    public class InvoiceReportGroup : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {

            public string PublicID { get; set; }

            public string GroupName { get; set; }

            public List<int> ListTransactionSouceID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
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
                sb.Append(@"SELECT InvoiceReportGroup.*,
                            InvoiceReportGroupDefn.PublicID as InvoiceReportGroupDefn_PublicID
                    FROM InvoiceReportGroup 
                        LEFT JOIN InvoiceReportGroupDefn ON InvoiceReportGroup.InvoiceReportGroupDefnID = InvoiceReportGroupDefn.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(PublicID))
                {
                    sb.Append(" AND InvoiceReportGroup.PublicID like '%' + @PublicID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PublicID", PublicID));
                }

                if (!string.IsNullOrEmpty(GroupName))
                {
                    sb.Append(" AND InvoiceReportGroup.GroupName like '%' + @GroupName + '%'");
                    ListSqlParameter.Add(new SqlParameter("@GroupName", GroupName));
                }

                if (BaseGenerateSqlQuery("InvoiceReportGroup",out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "InvoiceReportGroupDefnID is required")]
            public int InvoiceReportGroupDefnID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "PublicID is required")]
            public string PublicID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "TransactionSourceID is required")]
            public int TransactionSourceID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "GroupName is required")]
            public string GroupName { get; set; }
            public int SortOrder { get; set; }
        }
        public class dbRowExtended : dbRow
        {
            public string? InvoiceReportGroupDefn_PublicID { get; set; }
        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "InvoiceReportGroupDefnID", "PublicID", "TransactionSourceID", "GroupName" };
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
                        new ChangeTrackerData("InvoiceReportGroupDefnID", "InvoiceReportGroupDefn"),
                        new ChangeTrackerData("PublicID", "Public ID"),
                        new ChangeTrackerData("TransactionSourceID", "Transaction Source ID"),
                        new ChangeTrackerData("GroupName", "GroupName"),
                        new ChangeTrackerData("SortOrder", "Sort Order")
                    };
                }
                return _ListChangeTrackerData;
            }
        }
    }
}
