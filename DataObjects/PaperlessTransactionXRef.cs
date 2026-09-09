using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using wbm_common.Utility;
using static wbm_common.DataObjects.ChangeTracking;

namespace wbm_common.DataObjects
{
    public class PaperlessTransactionXref : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }

            public string PaperlessKeyID { get; set; }

            public List<int> PaperlessDataTypeID { get; set; }

            public string ReferenceNumber { get; set; }

            public int TransactionHeaderID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                PaperlessKeyID = 2,
                TransactionHeaderID = 3,
                AllRecords = 4,
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
                else if (SearchMode == SearchModeType.PaperlessKeyID)
                {
                    if (string.IsNullOrEmpty(PaperlessKeyID))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("PaperlessID", "PaperlessID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.TransactionHeaderID)
                {
                    if (TransactionHeaderID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("TransactionHeaderID", "TransactionHeaderID must be specified."));
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
                sb.Append("SELECT * FROM PaperlessTransactionXref WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(PaperlessKeyID))
                {
                    sb.Append(" AND PaperlessID = @PaperlessID");
                    ListSqlParameter.Add(new SqlParameter("@PaperlessID", PaperlessKeyID));
                }

                if (!string.IsNullOrEmpty(ReferenceNumber))
                {
                    sb.Append(" AND ReferenceNumber like '%' + @ReferenceNumber + '%'");
                    ListSqlParameter.Add(new SqlParameter("@ReferenceNumber", ReferenceNumber));
                }

                if (TransactionHeaderID > 0)
                {
                    sb.Append(" AND TransactionHeaderID = @TransactionHeaderID");
                    ListSqlParameter.Add(new SqlParameter("@TransactionHeaderID", TransactionHeaderID));
                }

                if (DataObjectSQLGenerator.ListPaperlessDataTypeID(PaperlessDataTypeID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("PaperlessTransactionXRef", out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "PaperlessKEYID is required")]
            public string PaperlessKeyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "PaperlessDataTypeID is required")]
            public int PaperlessDataTypeID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "ReferenceNumber is required")]
            public string ReferenceNumber { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "TransactionHeaderID is required")]
            public int TransactionHeaderID { get; set; }

        }
        public dbRow dbRow_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "PaperlessKeyID", "PaperlessDataTypeID", "ReferenceNumber", "TransactionHeaderID" };
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
                        new ChangeTrackerData("PaperlessKeyID", "Paperless Key ID"),
                        new ChangeTrackerData("PaperlessDataTypeID", "Paperless Data Type ID"),
                        new ChangeTrackerData("ReferenceNumber", "ReferenceNumber"),
                        new ChangeTrackerData("TransactionHeaderID", "Transaction Header ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }
    }
}
