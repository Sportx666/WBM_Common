using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class Company : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public string ShortName { get; set; }

            public string LongName { get; set; }

            public List<int> ListSystemID { get; set; }

            public Boolean? Active { get; set; }

            public string Abbreviation { get; set; }

            public string AbelCostCentre { get; set; }

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
                sb.Append(@"SELECT Company.*,
                        SiteSystem.PublicID AS SiteSystem_PublicID,
                        Security_User.PublicID as Created_UserPublicID
                        FROM Company 
                        LEFT JOIN SiteSystem ON Company.SystemID = SiteSystem.ID
                        LEFT JOIN Security_User on Company.CreatedUserID = Security_User.ID
                        WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrWhiteSpace(ShortName))
                {
                    sb.Append(" AND Company.ShortName = @ShortName");
                    ListSqlParameter.Add(new SqlParameter("@ShortName", ShortName));
                }

                if (!string.IsNullOrEmpty(LongName))
                {
                    sb.Append(" AND Company.LongName like '%' + @LongName + '%'");
                    ListSqlParameter.Add(new SqlParameter("@LongName", LongName));
                }

                if (DataObjectSQLGenerator.ListSystemID(ListSystemID, out generateSQL, out generateListParamater, "Company."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (Active.HasValue)
                {
                    sb.Append(" AND Company.Active = @Active");
                    ListSqlParameter.Add(new SqlParameter("@Active", Active.Value));
                }

                if (!string.IsNullOrEmpty(Abbreviation))
                {
                    sb.Append(" AND Company.Abbreviation = @Abbreviation");
                    ListSqlParameter.Add(new SqlParameter("@Abbreviation", Abbreviation));
                }

                if (!string.IsNullOrEmpty(AbelCostCentre))
                {
                    sb.Append(" AND Company.AbelCostCentre = @AbelCostCentre");
                    ListSqlParameter.Add(new SqlParameter("@AbelCostCentre", AbelCostCentre));
                }

                if (BaseGenerateSqlQuery("Company", out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "ShortName is required")]
            public string ShortName { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "LongName is required")]
            public string LongName { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SystemID is required")]
            public int SystemID { get; set; }
            [Required(ErrorMessage = "Active is required")]
            public Boolean Active { get; set; }
            public int SortOrder { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Abbreviation is required")]
            public string Abbreviation { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "AbelCostCentre is required")]
            public string AbelCostCentre { get; set; }

        }
        public dbRow dbRow_instance { get; set; }

        public class dbRowExtended : dbRow
        {
            public string? SiteSystem_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }


        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "ShortName", "LongName", "SystemID", "Active", "Abbreviation", "AbelCostCentre" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("ShortName", "Short Name"),
                        new ChangeTrackerData("LongName", "Long Name"),
                        new ChangeTrackerData("SystemID", "System ID"),
                        new ChangeTrackerData("Active", "Active"),
                        new ChangeTrackerData("SortOrder", "Sort Order"),
                        new ChangeTrackerData("Abbreviation", "Abbreviation"),
                        new ChangeTrackerData("AbelCostCentre", "Abel Cost Centre"),
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
