using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class Carrier : ChangeTracking
    {
        public ToDo ToDo { get; set; }
        public class Search : SearchCreatedLast_Base
        {
            public string PublicID { get; set; }

            public List<int> ListSiteID { get; set; }

            public string Description { get; set; }

            public string Paperless_KeyID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                Paperless_KeyID = 3,
                ListID = 4
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
                else if (SearchMode == SearchModeType.Paperless_KeyID)
                {
                    if (string.IsNullOrEmpty(Paperless_KeyID))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Paperless_KeyID", "Paperless_KeyID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.ListID)
                {
                    if (ListID == null || ListID.Count == 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ListID", "ListID must be populated."));
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
                //sb.Append("SELECT * FROM Carrier WHERE 1=1");
                sb.Append(@"
                        SELECT 
                            Carrier.*,
                            Site.PublicID AS Site_PublicID,
                            Security_User.PublicID as Created_UserPublicID
                        FROM Carrier
                        LEFT JOIN Site ON Carrier.SiteID = Site.ID
                        LEFT JOIN Security_User on Carrier.CreatedUserID = Security_User.ID
                        WHERE 1=1
                    ");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(PublicID))
                {
                    sb.Append(" AND Carrier.PublicID like '%' + @PublicID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PublicID", PublicID));
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND Carrier.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (!string.IsNullOrEmpty(Paperless_KeyID))
                {
                    sb.Append(" AND Carrier.Paperless_KeyID = @Paperless_KeyID");
                    ListSqlParameter.Add(new SqlParameter("@Paperless_KeyID", Paperless_KeyID));
                }

                if (DataObjectSQLGenerator.ListID(ListID, out generateSQL, out generateListParamater, "Carrier."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("Carrier", out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                SqlQuery = sb.ToString();
            }

        }

        public class dbRow : dbRowCreatedLast_Base
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "PublicID is required")]
            public string PublicID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            public int SortOrder { get; set; }

            public string Paperless_KeyID { get; set; }
        }

        public dbRow dbRow_instance { get; set; }


        public class dbRowExtended : dbRow
        {
            public string? Site_PublicID { get; set; }

            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }
        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "PublicID", "SiteID", "Description" };
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
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("SortOrder", "Sort Order"),
                        new ChangeTrackerData("Paperless_KeyID", "Paperless Key ID"),
                    };
                }
                return _ListChangeTrackerData;
            }
        }
    }
}
