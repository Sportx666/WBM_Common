using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class RateCode : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public string RateCode { get; set; }

            public string Description { get; set; }

            public List<int> ListRateTypeID { get; set; }

            public DateTime? DateOfChange_from { get; set; }
            public DateTime? DateOfChange_to { get; set; }

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
                sb.Append(@"SELECT RateCode.*,
                                 Security_User.PublicID as Created_UserPublicID
                            FROM RateCode
                                 LEFT JOIN Security_User on RateCode.CreatedUserID = Security_User.ID
                            WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;


                if (DataObjectSQLGenerator.ListRateTypeID(ListRateTypeID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(RateCode))
                {
                    sb.Append(" AND RateCode like '%' + @RateCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@RateCode", RateCode));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (DataObjectSQLGenerator.Date_FromTo("DateOfChange", DateOfChange_from, DateOfChange_to, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("RateCode", out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "RateCode is required")]
            public string RateCode { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "RateType is required")]
            public int RateType { get; set; }
            [Required(ErrorMessage = "MinimumCharge is required")]
            public Decimal MinimumCharge { get; set; }
            [Required(ErrorMessage = "Basic is required")]
            public Decimal Basic { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "IncludedUnits is required")]
            public int IncludedUnits { get; set; }

            public DateTime? DateOfChange { get; set; }
            [Required(ErrorMessage = "NewMinimumCharge is required")]
            public Decimal NewMinimumCharge { get; set; }
            [Required(ErrorMessage = "NewBasic is required")]
            public Decimal NewBasic { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "NewIncludedUnits is required")]
            public int NewIncludedUnits { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? RateType_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }


        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "RateCode", "RateType", "Description", "MinimumCharge"
                , "Basic", "IncludedUnits", "NewMinimumCharge", "NewBasic", "NewIncludedUnits" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("RateCode", "Rate Code"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("RateType", "Rate Type"),
                        new ChangeTrackerData("MinimumCharge", "Minimum Charge", true),
                        new ChangeTrackerData("Basic", "Basic", true),
                        new ChangeTrackerData("IncludedUnits", "Included Units"),
                        new ChangeTrackerData("DateOfChange", "Date Of Change"),
                        new ChangeTrackerData("NewMinimumCharge", "New Minimum Charge", true),
                        new ChangeTrackerData("NewBasic", "New Basic", true),
                        new ChangeTrackerData("NewIncludedUnits", "New Included Units")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
