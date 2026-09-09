using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace wbm_common.DataObjects
{
    public class AwaitingPickHeadDW : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public string KEYID { get; set; }
            public DateTime? InsertedDate_to { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                KeyID = 2,
                InsertedDate = 3,
                AllRecords = 4
            }

            public SearchModeType SearchMode { get; set; } = SearchModeType.Undefined;

            public System.Boolean IsValid()
            {
                ValidationErrors = new List<Tuple<string, string>>();

                System.Boolean rtnVal = true; // assume valid
                if (SearchMode == SearchModeType.SingleID)
                {
                    if (ID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ID", "Single ID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.KeyID)
                {
                    if (KEYID == null || KEYID == "")
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("KEYID", "KEYID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.InsertedDate)
                {
                    if (InsertedDate_to == null || InsertedDate_to < DateTime.Parse("01/01/2000"))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("InsertedDate", "InsertedDate_to must be specified."));
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
                sb.Append(@"SELECT * FROM AwaitingPickHeadDW WHERE 1=1 ");

                if (KEYID != null && KEYID != "")
                {
                    sb.Append(" AND KEYID = @KEYID");
                    ListSqlParameter.Add(new SqlParameter("@KEYID", KEYID));
                }
                if (InsertedDate_to != null && InsertedDate_to >= DateTime.Parse("01/01/2000"))
                {
                    sb.Append(" AND InsertedDate >= @InsertedDate");
                    ListSqlParameter.Add(new SqlParameter("@InsertedDate", InsertedDate_to));
                }

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                SqlQuery = sb.ToString();
            }

        }

        public bool delete = false;
        public dbRow dbRow_instance { get; set; }
        public class dbRow
        {
            [Range(0, int.MaxValue, ErrorMessage = "ID is required")]
            public int ID { get; set; }
            [Required(ErrorMessage = "InsertedDate is required")]
            public DateTime InsertedDate { get; set; }
            public DateTime? LastUpdatedDate { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "KEYID is required")]
            public string KEYID { get; set; }
        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "InsertedDate", "KEYID" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;
        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("LastUpdatedDate", "Last Updated Date")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
