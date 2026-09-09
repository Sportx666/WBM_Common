using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ContainerLift : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }

            public int? DefaultNumberOfLifts { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleOwnerID = 2,
                AllRecords = 3
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
                else if (SearchMode == SearchModeType.SingleOwnerID)
                {
                    if (ListOwnerID == null || ListOwnerID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Owner", "OwnerID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required
                    if (DataObjectValidator.IntGreaterZero("DefaultNumberOfLifts", DefaultNumberOfLifts, out ListLocalValidationErrors) == false)
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

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
                sb.Append(@"SELECT Containerlift.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            Security_User.PublicID as Created_UserPublicID
                    FROM Containerlift 
                        LEFT JOIN Site ON Containerlift.SiteID = Site.ID
                        LEFT JOIN Company ON Containerlift.CompanyID = Company.ID
                        LEFT JOIN Owner ON Containerlift.OwnerID = Owner.ID
                        LEFT JOIN Security_User on ContainerLift.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "Containerlift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "Containerlift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "Containerlift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DefaultNumberOfLifts.HasValue)
                {
                    sb.Append(" AND Containerlift.DefaultNumberOfLifts = @DefaultNumberOfLifts");
                    ListSqlParameter.Add(new SqlParameter("@DefaultNumberOfLifts", DefaultNumberOfLifts));
                }

                if (BaseGenerateSqlQuery("ContainerLift", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "DefaultNumberOfLifts is required")]
            public int DefaultNumberOfLifts { get; set; }

        }

        public dbRow dbRow_instance { get; set; }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "OwnerID", "DefaultNumberOfLifts" };
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
                        new ChangeTrackerData("DefaultNumberOfLifts", "Default Number Of Lifts")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
