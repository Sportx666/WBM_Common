using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Net;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class Owner : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public string SourceOwnerID { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }
            public string Suburb { get; set; }
            public string State { get; set; }
            public string PostCode { get; set; }

            public int CurrentRateCollectionDefnID { get; set; }
            public string CurrentRateCollectionPublicID { get; set; }

            public DateTime? DateOfChange_from { get; set; }
            public DateTime? DateOfChange_to { get; set; }

            public int NewRateCollectionDefnID { get; set; }
            public string NewRateCollectionPublicID { get; set; }
            public int CurrentProcessingRuleDefnID { get; set; }
            public string CurrentProcessingRulePublicID { get; set; }

            public DateTime? ProcessingRuleDateOfChange_from { get; set; }
            public DateTime? ProcessingRuleDateOfChange_to { get; set; }

            public int NewProcessingRuleDefnID { get; set; }
            public string NewProcessingRulePublicID { get; set; }

            public List<int> ListInvoiceCycleID { get; set; }

            public Boolean? Active { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleCompany = 3,
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
                else if (SearchMode == SearchModeType.SingleCompany)
                {
                    if (ListCompanyID == null || ListCompanyID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("CompanyID", "Single CompanyID must be specified."));
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
                sb.Append("SELECT * FROM Owner WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(SourceOwnerID))
                {
                    sb.Append(" AND SourceOwnerID like '%' + @SourceOwnerID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@SourceOwnerID", SourceOwnerID));
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    sb.Append(" AND Name like '%' + @Name + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Name", Name));
                }

                if (!string.IsNullOrEmpty(Address))
                {
                    sb.Append(" AND (AddressLine1 like '%' + @Address + '%'");
                    sb.Append(" OR AddressLine2 like '%' + @Address + '%'");
                    sb.Append(" OR AddressLine3 like '%' + @Address + '%'");
                    sb.Append(" OR AddressLine4 like '%' + @Address + '%')");

                    ListSqlParameter.Add(new SqlParameter("@Address", Address));
                }

                if (!string.IsNullOrEmpty(Suburb))
                {
                    sb.Append(" AND Suburb like '%' + @Suburb + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Suburb", Suburb));
                }

                if (!string.IsNullOrEmpty(State))
                {
                    sb.Append(" AND State like '%' + @State + '%'");
                    ListSqlParameter.Add(new SqlParameter("@State", State));
                }

                if (!string.IsNullOrEmpty(PostCode))
                {
                    sb.Append(" AND PostCode like '%' + @PostCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PostCode", PostCode));
                }

                if (CurrentRateCollectionDefnID > 0)
                {
                    sb.Append(" AND CurrentRateCollectionDefnID = @CurrentRateCollectionDefnID");
                    ListSqlParameter.Add(new SqlParameter("@CurrentRateCollectionDefnID", CurrentRateCollectionDefnID));
                }   

                if (!string.IsNullOrEmpty(CurrentRateCollectionPublicID))
                {
                    sb.Append(" AND CurrentRateCollectionDefnID IN (SELECT ID FROM RateCollectionDefn WHERE PublicID like '%' + @CurrentRateCollectionPublicID + '%')");
                    ListSqlParameter.Add(new SqlParameter("@CurrentRateCollectionPublicID", CurrentRateCollectionPublicID));
                }

                if (DataObjectSQLGenerator.Date_FromTo("DateOfChange", DateOfChange_from, DateOfChange_to, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (NewRateCollectionDefnID > 0)
                {
                    sb.Append(" AND NewRateCollectionDefnID = @NewRateCollectionDefnID");
                    ListSqlParameter.Add(new SqlParameter("@NewRateCollectionDefnID", NewRateCollectionDefnID));
                }

                if (!string.IsNullOrEmpty(NewRateCollectionPublicID))
                {
                    sb.Append(" AND NewRateCollectionDefnID IN (SELECT ID FROM RateCollectionDefn WHERE PublicID like '%' + @NewRateCollectionPublicID + '%')");
                    ListSqlParameter.Add(new SqlParameter("@NewRateCollectionPublicID", NewRateCollectionPublicID));
                }
                if (CurrentProcessingRuleDefnID > 0)
                {
                    sb.Append(" AND CurrentProcessingRuleDefnID = @CurrentProcessingRuleDefnID");
                    ListSqlParameter.Add(new SqlParameter("@CurrentProcessingRuleDefnID", CurrentProcessingRuleDefnID));
                }

                if (!string.IsNullOrEmpty(CurrentProcessingRulePublicID))
                {
                    sb.Append(" AND CurrentProcessingRuleDefnID IN (SELECT ID FROM ProcessingRuleDefn WHERE PublicID like '%' + @CurrentProcessingRulePublicID + '%')");
                    ListSqlParameter.Add(new SqlParameter("@CurrentProcessingRulePublicID", CurrentProcessingRulePublicID));
                }

                if (DataObjectSQLGenerator.Date_FromTo("ProcessingRuleDateOfChange", ProcessingRuleDateOfChange_from, ProcessingRuleDateOfChange_to, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (NewProcessingRuleDefnID > 0)
                {
                    sb.Append(" AND NewProcessingRuleDefnID = @NewProcessingRuleDefnID");
                    ListSqlParameter.Add(new SqlParameter("@NewProcessingRuleDefnID", NewProcessingRuleDefnID));
                }

                if (!string.IsNullOrEmpty(NewProcessingRulePublicID))
                {
                    sb.Append(" AND NewProcessingRuleDefnID IN (SELECT ID FROM ProcessingRuleDefn WHERE PublicID like '%' + @NewProcessingRulePublicID + '%')");
                    ListSqlParameter.Add(new SqlParameter("@NewProcessingRulePublicID", NewProcessingRulePublicID));
                }
                if (DataObjectSQLGenerator.ListInvoiceCycleID(ListInvoiceCycleID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (Active.HasValue)
                {
                    sb.Append(" AND Active = @Active");
                    ListSqlParameter.Add(new SqlParameter("@Active", Active.Value));
                }

                if (BaseGenerateSqlQuery("Owner", out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "SourceOwnerID is required")]
            public string SourceOwnerID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
            public string Name { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "AddressLine1 is required")]
            public string AddressLine1 { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "AddressLine2 is required")]
            public string AddressLine2 { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "AddressLine3 is required")]
            public string AddressLine3 { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "AddressLine4 is required")]
            public string AddressLine4 { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Suburb is required")]
            public string Suburb { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "State is required")]
            public string State { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "PostCode is required")]
            public string PostCode { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "CurrentRateCollectionDefnID is required")]
            public int CurrentRateCollectionDefnID { get; set; }

            public DateTime? DateOfChange { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "NewRateCollectionDefnID is required")]
            public int NewRateCollectionDefnID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "InvoiceCycleID is required")]
            public int InvoiceCycleID { get; set; }

            public DateTime? LastInvoiceDate { get; set; }

            public DateTime? RollBackInvoiceDate { get; set; }
            [Required(ErrorMessage = "Active is required")]
            public Boolean Active { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "CurrentProcessingRuleDefnID is required")]
            public int CurrentProcessingRuleDefnID { get; set; }

            public DateTime? ProcessingRuleDateOfChange { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "NewProcessingRuleDefnID is required")]
            public int NewProcessingRuleDefnID { get; set; }

            [NotMapped]
            public string OwnerNumberName
            {
                get
                {
                    return $"{SourceOwnerID} : {Name}";
                }

            }

        }
        public class dbRowExtended : dbRow
        {
            public string Company_ShortName { get; set; }
            public string CurrentRateCollectionDefn_PublicID { get; set; }
            public string NewRateCollectionDefn_PublicID { get; set; }
            public string InvoiceCycle_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRow dbRow_instance { get; set; }
        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SourceOwnerID", "Name", "AddressLine1",
                "AddressLine2", "AddressLine3", "AddressLine4", "Suburb", "State", "PostCode", "CurrentRateCollectionDefnID", 
                "NewRateCollectionDefnID", "InvoiceCycleID", "Active", "CurrentProcessingRuleDefnID", "NewProcessingRuleDefnID"};
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
                        new ChangeTrackerData("SourceOwnerID", "Source Owner ID"),
                        new ChangeTrackerData("Name", "Name"),
                        new ChangeTrackerData("AddressLine1", "Address Line 1"),
                        new ChangeTrackerData("AddressLine2", "Address Line 2"),
                        new ChangeTrackerData("AddressLine3", "Address Line 3"),
                        new ChangeTrackerData("AddressLine4", "Address Line 4"),
                        new ChangeTrackerData("Suburb", "Suburb"),
                        new ChangeTrackerData("State", "State"),
                        new ChangeTrackerData("PostCode", "Post Code"),
                        new ChangeTrackerData("CurrentRateCollectionDefnID", "Current Rate Collection Defn ID"),
                        new ChangeTrackerData("DateOfChange", "Rate Collection Date Of Change"),
                        new ChangeTrackerData("NewRateCollectionDefnID", "New Rate Collection Defn ID"),
                        new ChangeTrackerData("InvoiceCycleID", "Invoice Cycle ID"),
                        new ChangeTrackerData("LastInvoiceDate", "Last Invoice Date"),
                        new ChangeTrackerData("RolLBackInvoiceDate", "Rollback Invoice Date"),
                        new ChangeTrackerData("Active", "Active"),
                        new ChangeTrackerData("CurrentProcessingRuleDefnID", "Current Rate Collection Defn ID"),
                        new ChangeTrackerData("ProcessingRuleDateOfChange", "ProcessingRule Date Of Change"),
                        new ChangeTrackerData("NewProcessingRuleDefnID", "New ProcessingRule Defn ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
