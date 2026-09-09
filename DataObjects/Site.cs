using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class Site : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public string PublicID { get; set; }

            public List<int> ListSiteSystemID { get; set; }

            public List<int> ListCompanyID { get; set; }

            public Boolean? Active { get; set; }

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
                sb.Append(@"SELECT Site.*,
                            SiteSystem.PublicID AS SiteSystem_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Security_User.PublicID as Created_UserPublicID
                    FROM Site 
                        LEFT JOIN SiteSystem ON Site.SiteSystemID = SiteSystem.ID
                        LEFT JOIN Company ON Site.CompanyID = Company.ID
                        LEFT JOIN Security_User on Site.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(PublicID))
                {
                    sb.Append(" AND Site.PublicID like '%' + @PublicID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PublicID", PublicID));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND Site.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (DataObjectSQLGenerator.ListSiteSystemID(ListSiteSystemID, out generateSQL, out generateListParamater, "Site."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "Site."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (Active.HasValue)
                {
                    sb.Append(" AND Site.Active = @Active");
                    ListSqlParameter.Add(new SqlParameter("@Active", Active.Value));
                }

                if (BaseGenerateSqlQuery("Site", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "SiteSystemID is required")]
            public int SiteSystemID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            public Boolean Active { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            public int SortOrder { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? SiteSystem_PublicID { get; set; }
            public string? Company_ShortName { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "PublicID", "SiteSystemID", "Description", "CompanyID" };
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
                        new ChangeTrackerData("SiteSystemID", "Site System ID"),
                        new ChangeTrackerData("CompanyID", "Company ID"),
                        new ChangeTrackerData("Active", "Active"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("SortOrder", "Sort Order")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
