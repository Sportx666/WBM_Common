using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class Location : ChangeTracking
    {
        public ToDo ToDo { get; set; }

        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListSiteID { get; set; }

            public string PublicLocationID { get; set; }

            public List<int> ListPalletStorageTypeID { get; set; }

            public string Paperless_KeyID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                SingleCompany = 3,
                SingleSite = 4,
                Paperless_KeyID = 5
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
                else if (SearchMode == SearchModeType.SingleSite)
                {
                    if (ListSiteID == null || ListSiteID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Site", "Single SiteID must be specified."));
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
                sb.Append(@"SELECT Location.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            PalletStorageType.PublicID AS PalletStorageType_PublicID,
                            Security_User.PublicID as Created_UserPublicID
                    FROM Location 
                        LEFT JOIN Site ON Location.SiteID = Site.ID
                        LEFT JOIN Company ON Location.CompanyID = Company.ID
                        LEFT JOIN PalletStorageType ON Location.PalletStorageTypeID = PalletStorageType.ID
                        LEFT JOIN Security_User on Location.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "Location."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "Location."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(PublicLocationID))
                {
                    sb.Append(" AND Location.PublicLocationID like '%' + @PublicLocationID+ '%'");
                    ListSqlParameter.Add(new SqlParameter("@PublicLocationID", PublicLocationID));
                }

                if (DataObjectSQLGenerator.ListPalletStorageTypeID(ListPalletStorageTypeID, out generateSQL, out generateListParamater, "Location."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Paperless_KeyID))
                {
                    sb.Append(" AND Location.Paperless_KeyID = @Paperless_KeyID");
                    ListSqlParameter.Add(new SqlParameter("@Paperless_KeyID", Paperless_KeyID));
                }

                if (BaseGenerateSqlQuery("Location", out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "PublicLocationID is required")]
            public string PublicLocationID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "PalletStorageTypeID is required")]
            public int PalletStorageTypeID { get; set; }

            public string Paperless_KeyID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Site_PublicID { get; set; }
            public string? PalletStorageType_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "PublicLocationID", "PalletStorageTypeID" };
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
                        new ChangeTrackerData("PublicLocationID", "Public Location ID"),
                        new ChangeTrackerData("PalletStorageTypeID", "Pallet Storage Type ID"),
                        new ChangeTrackerData("Paperless_KeyID", "Paperless Key ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
