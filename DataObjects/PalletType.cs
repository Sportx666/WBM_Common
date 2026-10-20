using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class PalletType : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListSiteID { get; set; }
            public List<int> ListSiteSystemID { get; set; }
            public string PublicID { get; set; }

            public string Description { get; set; }

            public Boolean? ApplyPalletHire { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                SingleSystem = 3,
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
                else if (SearchMode == SearchModeType.SingleSystem)
                {
                    if (ListSiteSystemID == null || ListSiteSystemID.Count > 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ListSiteSystemID", "Single SystemID must be specified."));
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

                sb.Append(@"SELECT PalletType.*,
                            Site.PublicID AS Site_PublicID,
                            Security_User.PublicID as Created_UserPublicID,
                            SiteSystem.PublicID AS SiteSystem_PublicID
                    FROM PalletType 
                        LEFT JOIN Site ON PalletType.SiteID = Site.ID
                        LEFT JOIN Security_User on PalletType.CreatedUserID = Security_User.ID
                        LEFT JOIN SiteSystem ON PalletType.SiteSystemID = SiteSystem.ID
                    WHERE 1=1");


                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(PublicID))
                {
                    sb.Append(" AND PalletType.PublicID like '%' + @PublicID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PublicID", PublicID));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND PalletType.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (ApplyPalletHire != null)
                {
                    sb.Append(" AND PalletType.ApplyPalletHire = " + (ApplyPalletHire.Value ? "1 " : "0 "));
                    ListSqlParameter.Add(new SqlParameter("@ApplyPalletHire", ApplyPalletHire));
                }

                if (DataObjectSQLGenerator.ListSiteSystemID(ListSiteSystemID, out generateSQL, out generateListParamater, "PalletType."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "PalletType."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }


                if (BaseGenerateSqlQuery("PalletType", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteSystemID is required")]
            public int SiteSystemID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "PublicID is required")]
            public string PublicID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Required(ErrorMessage = "ApplyPalletHire is required")]
            public bool ApplyPalletHire { get; set; }
            public int SortOrder { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Site_PublicID { get; set; }
            public string? SiteSystem_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }
        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "SiteID", "SiteSystemID", "PublicID", "Description", "ApplyPalletHire"};
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("SiteID", "Site  ID"),
                        new ChangeTrackerData("SiteSystemID", "Site System ID"),
                        new ChangeTrackerData("PublicID", "Public ID"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("ApplyPalletHire", "Apply Pallet Hire"),
                        new ChangeTrackerData("SortOrder", "Sort Order")
                    };
                }
                return _ListChangeTrackerData;
            }
        }
    }
}
