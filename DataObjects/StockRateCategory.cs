using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class StockRateCategory : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }

            public string PublicID { get; set; }

            public List<int> ListSiteID { get; set; }

            public string Description { get; set; }
            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleSite = 2,
                SingleCompany = 3,
                AllRecords = 4,
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
                        ValidationErrors.Add(new Tuple<string, string>("Company", "CompanyID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.SingleSite)
                {
                    if (ListSiteID == null || ListSiteID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Site", "SiteID must be specified."));
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
                sb.Append(@"SELECT StockRateCategory.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Security_User.PublicID as Created_UserPublicID
                    FROM StockRateCategory 
                        LEFT JOIN Site ON StockRateCategory.SiteID = Site.ID
                        LEFT JOIN Company ON StockRateCategory.CompanyID = Company.ID
                        LEFT JOIN Security_User on StockRateCategory.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "StockRateCategory."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "StockRateCategory."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(PublicID))
                {
                    sb.Append(" AND StockRateCategory.PublicID like '%' + @PublicID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PublicID", PublicID));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND StockRateCategory.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (BaseGenerateSqlQuery("StockRateCategory", out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "PublicID is required")]
            public string PublicID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            public int SortOrder { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Site_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "PublicID", "SiteID", "Description", "CompanyID" };
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
                        new ChangeTrackerData("PublicID", "Public ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("SortOrder", "Sort Order")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
