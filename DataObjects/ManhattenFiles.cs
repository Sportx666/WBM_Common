using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ManhattenFiles : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }

            public string Filename { get; set; }

            public List<int> ListFileTypeID { get; set; }

            public DateTime? DateLoaded_from { get; set; }
            public DateTime? DateLoaded_to { get; set; }


            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                SingleCompany = 3,
                SingleSite = 4,
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
                sb.Append(@"SELECT ManhattenFiles.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            ManhattenFileType.Description AS ManhattenFileType_PublicID,
                            Security_User.PublicID as Created_UserPublicID
                    FROM ManhattenFiles 
                        LEFT JOIN Site ON ManhattenFiles.SiteID = Site.ID
                        LEFT JOIN Company ON ManhattenFiles.CompanyID = Company.ID
                        LEFT JOIN Owner ON ManhattenFiles.OwnerID = Owner.ID
                        LEFT JOIN ManhattenFileType ON ManhattenFiles.FileTypeID = ManhattenFileType.ID
                        LEFT JOIN Security_User on ManhattenFiles.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "ManhattenFiles."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "ManhattenFiles."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "ManhattenFiles."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Filename))
                {
                    sb.Append(" AND ManhattenFiles.Filename like '%' + @Filename + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Filename", Filename));
                }

                if (DataObjectSQLGenerator.ListFileTypeID(ListFileTypeID, out generateSQL, out generateListParamater, "ManhattenFiles."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("DateLoaded", DateLoaded_from, DateLoaded_to, out generateSQL, out generateListParamater, "ManhattenFiles."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("ManhattenFiles",out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Filename is required")]
            public string Filename { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "FileTypeID is required")]
            public int FileTypeID { get; set; }
            [Required(ErrorMessage = "DateLoaded is required")]
            public DateTime DateLoaded { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "NumberOfLines is required")]
            public int NumberOfLines { get; set; }
        }

        public dbRow dbRow_instance { get; set; }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? ManhattenFileType_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }
        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "OwnerID", "Filename", "FileTypeID", "DateLoaded", "NumberOfLines" };
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
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("Filename", "Filename"),
                        new ChangeTrackerData("FileTypeID", "File Type ID"),
                        new ChangeTrackerData("DateLoaded", "Date Loaded"),
                        new ChangeTrackerData("NumberOfLines", "Number Of Lines")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
