using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace wbm_common.DataObjects
{
    public class InvoiceCycle : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public string PublicID { get; set; }
            public string Description { get; set; }

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

                    if (!BaseIsValid(out List<Tuple<string, string>> ListLocalValidationErrors))
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
                sb.Append(@"SELECT InvoiceCycle.*,
                                Security_User.PublicID as Created_UserPublicID
                            FROM InvoiceCycle 
                                 LEFT JOIN Security_User on InvoiceCycle.CreatedUserID = Security_User.ID
                            WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(PublicID))
                {
                    sb.Append(" AND PublicID like '%' + @PublicID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PublicID", PublicID));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (BaseGenerateSqlQuery("InvoiceCycle",out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "PublicID is required")]
            public string PublicID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "IncrementDays is required")]
            public int IncrementDays { get; set; }
            [Required(ErrorMessage = "AllowUserSetNextDate is required")]
            public Boolean AllowUserSetNextDate { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "CalculateNextDateRuleID is required")]
            public int CalculateNextDateRuleID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "PublicID", "IncrementDays", "Description", "AllowUserSetNextDate", "CalculateNextDateRuleID" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("PublicID", "Public ID"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("IncrementDays", "Increment Days"),
                        new ChangeTrackerData("AllowUserSetNextDate", "Allow User Set Next Date"),
                        new ChangeTrackerData("CalculateNextDateRuleID", "Calculate Next Date Rule ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
