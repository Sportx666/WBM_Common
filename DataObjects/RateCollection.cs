using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class RateCollection : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListRateFunctionID { get; set; }

            public List<int> ListRateCollectionDefnID { get; set; }

            public string RateChargeCode { get; set; }

            public List<int> ListRateCodeID { get; set; }

            public string Description { get; set; }

            public DateTime? DateOfChange_from { get; set; }
            public DateTime? DateOfChange_to { get; set; }
            public List<int> ListNewRateCodeID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleCompanyID = 3,
                AllRecords = 2,
                SingleRateCollectionDefnID = 4
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
                        ValidationErrors.Add(new Tuple<string, string>("CompanyID", "Single Company ID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.SingleRateCollectionDefnID)
                {
                    if (ListRateCollectionDefnID == null || ListRateCollectionDefnID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("RateCollectionDefnID", "Single RateCollectionDefnID must be specified."));
                    }
                }

                else
                {
                    // Validate each field as required

                    if (DataObjectValidator.DateMinMax("DateOfChange", DateOfChange_from, DateOfChange_to, out ListLocalValidationErrors) == false)
                    {
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
                sb.Append(@"SELECT RateCollection.*,
                            Company.ShortName AS Company_ShortName,
                            RateCollectionDefn.PublicID AS RateCollectionDefn_PublicID,
                            RateFunction.PublicID AS RateFunction_PublicID,
                            RC.RateCode AS RateCode_RateCode,
                            NRC.RateCode AS NewRateCode_RateCode,
                            Security_User.PublicID as Created_UserPublicID
                    FROM RateCollection 
                        LEFT JOIN RateCollectionDefn ON RateCollection.RateCollectionDefnID = RateCollectionDefn.ID
                        LEFT JOIN RateFunction ON RateCollection.RateFunctionID = RateFunction.ID
                        LEFT JOIN RateCode RC ON RateCollection.RateCodeID = RC.ID
                        LEFT JOIN RateCode NRC ON RateCollection.NewRateCodeID = NRC.ID
                        LEFT JOIN Company ON RateCollection.CompanyID = Company.ID
                        LEFT JOIN Security_User on RateCollection.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "RateCollection."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListRateFunctionID(ListRateFunctionID, out generateSQL, out generateListParamater, "RateCollection."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListRateCodeID(ListRateCodeID, out generateSQL, out generateListParamater, "RateCollection."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND RateCollection.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (DataObjectSQLGenerator.Date_FromTo("DateOfChange", DateOfChange_from, DateOfChange_to, out generateSQL, out generateListParamater, "RateCollection."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }   

                if (DataObjectSQLGenerator.ListNewRateCodeID(ListNewRateCodeID, out generateSQL, out generateListParamater, "RateCollection."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("RateCollection",out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "RateCollectionDefnID is required")]
            public int RateCollectionDefnID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "RateChargeCode is required")]
            public string RateChargeCode { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "RateFunctionID is required")]
            public int RateFunctionID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "RateCodeID is required")]
            public int RateCodeID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SecondarySort is required")]
            public int SecondarySort { get; set; }

            public DateTime? DateOfChange { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "NewRateCodeID is required")]
            public int NewRateCodeID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? RateFunction_PublicID { get; set; }
            public string? RateCode_RateCode { get; set; }
            public string? RateCollectionDefn_PublicID { get; set; }
            public string? NewRateCode_RateCode { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "RateCollectionDefnID", "RateChargeCode", "RateFunctionID"
                , "RateCodeID", "Description", "SecondarySort", "NewRateCodeID" };
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
                        new ChangeTrackerData("RateFunctionID", "Rate Function ID"),
                        new ChangeTrackerData("RateCollectionDefnID", "Rate Collection Defn ID"),
                        new ChangeTrackerData("Rate Charge Code", "Rate Charge Code"),
                        new ChangeTrackerData("RateCodeID", "Rate Code ID"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("SecondarySort", "Secondary Sort"),
                        new ChangeTrackerData("DateOfChange", "Date Of Change"),
                        new ChangeTrackerData("NewRateCodeID", "New Rate Code ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
