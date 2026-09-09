using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ZoneProcessing : ChangeTracking
    {
        public ToDo ToDo { get; set; }
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }

            public List<int> ListSiteID { get; set; }

            public string ZoneNumber { get; set; }

            public string Description { get; set; }

            public Boolean? IsPickFace { get; set; }

            public Boolean? IsReserve { get; set; }

            public Boolean? IsCartonConsolidation { get; set; }

            public Boolean? IsReceiving { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleSiteID = 2,
                SingleCompanyID = 3,
                AllRecords = 4,
                SiteZoneNumber = 5
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
                else if (SearchMode == SearchModeType.SingleCompanyID)
                {
                    if (ListCompanyID == null || ListCompanyID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Company", "CompanyID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.SingleSiteID)
                {
                    if (ListSiteID == null || ListSiteID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Site", "SiteID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.SiteZoneNumber)
                {
                    if (ListSiteID == null || ListSiteID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Site", "SiteID must be specified."));
                    }
                    if (string.IsNullOrEmpty(ZoneNumber))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ZoneNumber", "ZoneNumber must be specified."));
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
                sb.Append(@"SELECT ZoneProcessing.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Security_User.PublicID as Created_UserPublicID
                    FROM ZoneProcessing 
                        LEFT JOIN Site ON ZoneProcessing.SiteID = Site.ID
                        LEFT JOIN Company ON ZoneProcessing.CompanyID = Company.ID
                        LEFT JOIN Security_User on ZoneProcessing.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "ZoneProcessing."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "ZoneProcessing."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(ZoneNumber))
                {
                    sb.Append(" AND ZoneProcessing.ZoneNumber like '%' + @ZoneNumber + '%'");
                    ListSqlParameter.Add(new SqlParameter("@ZoneNumber", ZoneNumber));
                }

                if(IsPickFace.HasValue)
                {
                    sb.Append(" AND ZoneProcessing.IsPickFace = @IsPickFace");
                    ListSqlParameter.Add(new SqlParameter("@IsPickFace", IsPickFace.Value));
                }

                if (IsReserve.HasValue)
                {
                    sb.Append(" AND ZoneProcessing.IsReserve = @IsReserve");
                    ListSqlParameter.Add(new SqlParameter("@IsReserve", IsReserve.Value));
                }

                if (IsCartonConsolidation.HasValue)
                {
                    sb.Append(" AND ZoneProcessing.IsCartonConsolidation = @IsCartonConsolidation");
                    ListSqlParameter.Add(new SqlParameter("@IsCartonConsolidation", IsCartonConsolidation.Value));
                }

                if (IsReceiving.HasValue)
                {
                    sb.Append(" AND ZoneProcessing.IsReceiving = @IsReceiving");
                    ListSqlParameter.Add(new SqlParameter("@IsReceiving", IsReceiving.Value));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND ZoneProcessing.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Desciption", Description));
                }

                if (BaseGenerateSqlQuery("ZoneProcessing",out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "ZoneNumber is required")]
            public string ZoneNumber { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "IsPickFace is required")]
            public Boolean IsPickFace { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "IsReserve is required")]
            public Boolean IsReserve { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "IsCartonConsolidation is required")]
            public Boolean IsCartonConsolidation { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "IsReceiving is required")]
            public Boolean IsReceiving { get; set; }
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
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "ZoneNumber", "Description"
                , "IsPickFace", "IsReserve", "IsCartonConsolidation", "IsReceiving" };
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
                        new ChangeTrackerData("ZoneNumber", "Zone Number"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("IsPickFace", "Is Pick Face"),
                        new ChangeTrackerData("IsReserve", "Is Reserve"),
                        new ChangeTrackerData("IsCartonConsolidation", "Is Carton Consolidation"),
                        new ChangeTrackerData("IsReceiving", "Is Receiving")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
