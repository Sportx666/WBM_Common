using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class TableRate : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListRateID { get; set; }

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

                    if (!DataObjectValidator.DateMinMax("DateOfChange", DateOfChange_from, DateOfChange_to, out ListLocalValidationErrors))
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
                sb.Append(@"SELECT TableRate.*,
                            RateCode.RateCode AS RateID_RateCode,
                            Security_User.PublicID as Created_UserPublicID
                    FROM TableRate 
                        LEFT JOIN RateCode ON TableRate.RateID = RateCode.ID
                        LEFT JOIN Security_User on TableRate.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListRateID(ListRateID, out generateSQL, out generateListParamater, "TableRate."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("DateOfChange", DateOfChange_from, DateOfChange_to, out generateSQL, out generateListParamater, "TableRate."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("TableRate", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "RateID is required")]
            public int RateID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "TableBreak is required")]
            public int TableBreak { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "TableUnits is required")]
            public int TableUnits { get; set; }
            [Required(ErrorMessage = "TableCharge is required")]
            public Decimal TableCharge { get; set; }

            public DateTime? DateOfChange { get; set; }
            [Required(ErrorMessage = "NewTableCharge is required")]
            public Decimal NewTableCharge { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? RateID_RateCode { get; set; }
            public string? Created_UserPublicID { get; set; }
        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "RateID", "TableBreak", "TableUnits", "TableCharge"
                , "NewTableCharge" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("RateID", "Rate ID"),
                        new ChangeTrackerData("TableBreak", "Table Break"),
                        new ChangeTrackerData("TableUnits", "Table Units"),
                        new ChangeTrackerData("TableCharge", "Table Charge", true),
                        new ChangeTrackerData("DateOfChange", "Date Of Change", true),
                        new ChangeTrackerData("NewTableCharge", "New Table Charge")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
