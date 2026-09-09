using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class InvoiceCycleHistory : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListInvoiceCycleID { get; set; }

            public DateTime? PeriodEndingDate_from { get; set; }
            public DateTime? PeriodEndingDate_to { get; set; }

            public string Description { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                SingleCompany = 3
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
                else if (SearchMode == SearchModeType.SingleCompany)
                {
                    if (ListCompanyID == null || ListCompanyID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Company", "Single CompanyID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

                    if (!DataObjectValidator.DateMinMax("PeriodEndingDate", PeriodEndingDate_from, PeriodEndingDate_to, out ListLocalValidationErrors))
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
                sb.Append(@"SELECT InvoiceCycleHistory.*,
                            InvoiceCycle.PublicID AS InvoiceCycle_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Security_User.PublicID as Created_UserPublicID
                    FROM InvoiceCycleHistory 
                        LEFT JOIN InvoiceCycle ON InvoiceCycleHistory.InvoiceCycleID = InvoiceCycle.ID
                        LEFT JOIN Company ON InvoiceCycleHistory.CompanyID = Company.ID
                        LEFT JOIN Security_User on InvoiceCycleHistory.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "InvoiceCycleHistory."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListInvoiceCycleID(ListInvoiceCycleID, out generateSQL, out generateListParamater, "InvoiceCycleHistory."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("PeriodEndingDate", PeriodEndingDate_from, PeriodEndingDate_to, out generateSQL, out generateListParamater, "InvoiceCycleHistory."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (BaseGenerateSqlQuery("InvoiceCycleHistory",out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "InvoiceCycleID is required")]
            public int InvoiceCycleID { get; set; }
            [Required(ErrorMessage = "PeriodEndingDate is required")]
            public DateTime PeriodEndingDate { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }

            public dbRow ()
            {

            }

            public dbRow(InvoiceCycleCurrent.dbRow ICCRow, string Description)
            {
                this.CompanyID = ICCRow.CompanyID;
                this.InvoiceCycleID = ICCRow.InvoiceCycleID;
                this.PeriodEndingDate = ICCRow.CurrentPeriodEnding;
                this.Description = Description;
                this.DeletedFlag = false;
            }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? InvoiceCycle_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "InvoiceCycleID", "PeriodEndingDate", "Description" };
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
                        new ChangeTrackerData("InvoiceCycleID", "Invoice Cycle ID"),
                        new ChangeTrackerData("PeriodEndingDate", "Period Ending Date"),
                        new ChangeTrackerData("Description", "Description")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
