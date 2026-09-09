using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ToDo : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCategoryID { get; set; }

            public List<int> ListSiteID { get; set; }
            public List<int> ListOwnerID { get; set; }

            public DateTime? EventDate_from { get; set; }

            public DateTime? EventDate_to { get; set; }

            public string Description { get; set; }

            public List<int> ListChangeHeaderTableID { get; set; }

            public List<int> ListSourceID { get; set; }

            public List<int> ListStatusID { get; set; }

            public string Note { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1
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
                else
                {
                    // Validate each field as required

                    if (DataObjectValidator.DateMinMax("EventDateTime", EventDate_from, EventDate_to, out ListLocalValidationErrors) == false)
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
                sb.Append(@"SELECT ToDo.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            ToDo_Category.PublicID AS ToDoCategory_PublicID,
                            ChangeHeader_TableName.TableName AS ChangeHeaderTable_Name,
                            ToDoStatus_Type.PublicID AS ToDoStatusType_PublicID,
                            Security_User.PublicID as Created_UserPublicID
                    FROM ToDo 
                        LEFT JOIN Site ON ToDo.SiteID = Site.ID
                        LEFT JOIN Company ON ToDo.CompanyID = Company.ID
                        LEFT JOIN Owner ON ToDo.OwnerID = Owner.ID
                        LEFT JOIN ToDo_Category ON ToDo.CategoryID = ToDo_Category.ID
                        LEFT JOIN ChangeHeader_TableName ON ToDo.ChangeHeaderTableID = ChangeHeader_TableName.ID
                        LEFT JOIN ToDoStatus_Type ON ToDo.StatusID = ToDoStatus_Type.ID
                        LEFT JOIN Security_User on ToDo.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCategoryID(ListCategoryID, out generateSQL, out generateListParamater, "ToDo."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if(DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "ToDo."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "ToDo."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("EventDateTime", EventDate_from, EventDate_to, out generateSQL, out generateListParamater, "ToDo."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND ToDo.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (!DataObjectSQLGenerator.ListChangeHeaderTableID(ListChangeHeaderTableID, out generateSQL, out generateListParamater, "ToDo."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListStatusID(ListStatusID, out generateSQL, out generateListParamater, "ToDo."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSourceID(ListSourceID, out generateSQL, out generateListParamater, "ToDo."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Note))
                {
                    sb.Append(" AND ToDo.Note like '%' + @Note + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Note", Note));
                }

                if (BaseGenerateSqlQuery("ToDo", out generateSQL, out generateListParamater))
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
            [Range(0, int.MaxValue, ErrorMessage = "CategoryID is required")]
            public int CategoryID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "EventDateTime is required")]
            public DateTime EventDateTime { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ChangeHeaderTableID is required")]
            public int ChangeHeaderTableID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "SourceID is required")]
            public int SourceID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "StatusID is required")]
            public int StatusID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Note is required")]
            public string Note { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? ToDoCategory_PublicID { get; set; }
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? ChangeHeaderTable_Name { get; set; }
            public string? ToDoStatusType_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CategoryID", "SiteID", "CompanyID", "OwnerID"
                , "EventDateTime", "Description", "ChangeHeaderTableID", "SourceID", "StatusID", "Note" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("CategoryID", "Category ID"),
                        new ChangeTrackerData("CompanyID", "Company ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("EventDateTime", "Event DateTime"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("ChangeHeaderTableID", "Change Header Table ID"),
                        new ChangeTrackerData("SourceID", "Source ID"),
                        new ChangeTrackerData("StatusID", "Status ID"),
                        new ChangeTrackerData("Note", "Note")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
