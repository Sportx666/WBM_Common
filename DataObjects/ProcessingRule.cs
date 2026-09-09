using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ProcessingRule : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListSiteID { get; set; }

            public int ProcessingRuleDefnID { get; set; }

            public List<int> ListTriggerID { get; set; }

            public List<int> ListProcessID { get; set; }

            public List<int> ListRateFunctionID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                ProcessingRuleDefnID = 3
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
                else if (SearchMode == SearchModeType.ProcessingRuleDefnID)
                {
                    if (ProcessingRuleDefnID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ProcessingRuleDefnID", "Single ProcessingRuleDefnID must be specified."));
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
                sb.Append(@"SELECT ProcessingRule.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            ProcessingRuleDefn.PublicID AS ProcessingRuleDefn_PublicID,
                            ProcessCategory.PublicID AS ProcessCategory_PublicID,
                            Process.PublicID AS Process_PublicID,
                            RateFunction.PublicID AS RateFunction_PublicID,
                            ProcessingRuleProcess.PublicID AS ProcessingRuleProcess_PublicID,
                            ProcessingRuleTriggerCondition.PublicID AS ProcessingRuleTriggerCondition_PublicID,
                            Security_User.PublicID as Created_UserPublicID,
                            0 AS TriggerID
                    FROM ProcessingRule 
                        LEFT JOIN Site ON ProcessingRule.SiteID = Site.ID
                        LEFT JOIN Company ON ProcessingRule.CompanyID = Company.ID
                        LEFT JOIN ProcessingRuleDefn ON ProcessingRule.ProcessingRuleDefnID = ProcessingRuleDefn.ID
                        LEFT JOIN ProcessCategory ON ProcessingRule.ProcessCategoryID = ProcessCategory.ID
                        LEFT JOIN Process ON ProcessingRule.ProcessID = Process.ID
                        LEFT JOIN RateFunction ON ProcessingRule.RateFunctionID = RateFunction.ID
                        LEFT JOIN ProcessingRuleProcess ON ProcessingRule.ProcessID = ProcessingRuleProcess.ID
                        LEFT JOIN ProcessingRuleTriggerCondition ON ProcessingRule.ProcessID = ProcessingRuleTriggerCondition.ID
                        LEFT JOIN Security_User on ProcessingRule.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;


                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "ProcessingRule."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "ProcessingRule."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (ProcessingRuleDefnID > 0)
                {
                    sb.Append(" AND ProcessingRule.ProcessingRuleDefnID = @ProcessingRuleDefnID");
                    ListSqlParameter.Add(new SqlParameter("@ProcessingRuleDefnID", ProcessingRuleDefnID));
                }

                if (DataObjectSQLGenerator.ListTriggerID(ListTriggerID, out generateSQL, out generateListParamater, "ProcessingRule."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListProcessID(ListProcessID, out generateSQL, out generateListParamater, "ProcessingRule."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListRateFunctionID(ListRateFunctionID, out generateSQL, out generateListParamater, "ProcessingRule."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("ProcessingRule", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ProcessingRuleDefnID is required")]
            public int ProcessingRuleDefnID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ProcessCategoryID is required")]
            public int ProcessCategoryID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ProcessID is required")]
            public int ProcessID { get; set; }
            public int SortOrder { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "RateFunctionID is required")]
            public int RateFunctionID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public int? TriggerID { get; set; }
            public string? Company_ShortName { get; set; }
            public string? ProcessingRuleDefn_PublicID { get; set; }
            public string? Site_PublicID { get; set; }
            public string? ProcessingRuleTriggerCondition_PublicID { get; set; }
            public string? ProcessingRuleProcess_PublicID { get; set; }
            public string? ProcessCategory_PublicID { get; set; }
            public string? Process_PublicID { get; set; }
            public string? RateFunction_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "ProcessingRuleDefnID", "ProcessCategoryID",
                "ProcessID", "RateFunctionID" };
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
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("ProcessingRuleDefnID", "Processing Rule Defn ID"),
                        new ChangeTrackerData("ProcessCategoryID", "Process Category ID"),
                        new ChangeTrackerData("ProcessID", "Process ID"),
                        new ChangeTrackerData("SortOrder", "Sort Order"),
                        new ChangeTrackerData("RateFunctionID", "Rate Function ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
