using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;
using static wbm_common.DataObjects.ChangeTracking;

namespace wbm_common.DataObjects
{
    public class ActionRateCode : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }
            public bool? IsPriority { get; set; }
            public List<int> ListActionTypeID { get; set; }
            public List<int> ListUnitTypeID { get; set; }
            public List<int> ListStockRateCategoryID { get; set; }
            public Decimal? WeightBreak_min { get; set; }
            public Decimal? WeightBreak_max { get; set; }
            public Decimal? LengthBreak_min { get; set; }
            public Decimal? LengthBreak_max { get; set; }
            public string RateCollectionRateChargeCode { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                SingleOwner = 3
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
                else if (SearchMode == SearchModeType.SingleOwner)
                {
                    if (ListOwnerID == null || ListOwnerID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Owner", "Single Owner must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

                    if (!DataObjectValidator.DecimalMinMax("Weight Break", WeightBreak_min, WeightBreak_max, out ListLocalValidationErrors))
                    {
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (!DataObjectValidator.DecimalMinMax("Length Break", LengthBreak_min, LengthBreak_max, out ListLocalValidationErrors))
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
                sb.Append(@"SELECT ActionRateCode.*,
                        Company.ShortName AS Company_ShortName,
                        Owner.Name AS Owner_Name,
                        Site.PublicID AS Site_PublicID,
                        ActionType.PublicID AS ActionType_PublicID,
                        UnitType.PublicID AS UnitType_PublicID,
                        StockRateCategory.PublicID AS StockRateCategory_PublicID,
                        Security_User.PublicID as Created_UserPublicID
                    FROM ActionRateCode
                    LEFT JOIN Company ON ActionRateCode.CompanyID = Company.ID
                    LEFT JOIN Owner ON ActionRateCode.OwnerID = Owner.ID
                    LEFT JOIN Site ON ActionRateCode.SiteID = Site.ID
                    LEFT JOIN ActionType ON ActionRateCode.ActionTypeID = ActionType.ID
                    LEFT JOIN UnitType ON ActionRateCode.UnitTypeID = UnitType.ID
                    LEFT JOIN StockRateCategory ON ActionRateCode.StockRateCategoryID = StockRateCategory.ID
                    LEFT JOIN Security_User on ActionRateCode.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (IsPriority.HasValue)
                {
                    sb.Append(" AND ActionRateCode.IsPriority = @IsPriority");
                    ListSqlParameter.Add(new SqlParameter("@IsPriority", IsPriority.Value));
                }

                if (DataObjectSQLGenerator.ListActionTypeID(ListActionTypeID, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListUnitTypeID(ListUnitTypeID, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListStockRateCategoryID(ListStockRateCategoryID, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("WeightBreak", WeightBreak_min, WeightBreak_max, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("LengthBreak", LengthBreak_min, LengthBreak_max, out generateSQL, out generateListParamater, "ActionRateCode."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(RateCollectionRateChargeCode))
                {
                    sb.Append(" AND RateCollectionRateChargeCode like '%' + @RateCollectionRateChargeCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@RateCollectionRateChargeCode", RateCollectionRateChargeCode));
                }

                if (BaseGenerateSqlQuery("ActionRateCode", out generateSQL, out generateListParamater))
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
            public bool? IsPriority { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ActionTypeID is required")]
            public int ActionTypeID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "UnitTypeID is required")]
            public int UnitTypeID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "StockRateCategoryID is required")]
            public int StockRateCategoryID { get; set; }
            public Decimal WeightBreak { get; set; }
            public Decimal LengthBreak { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "RateCollectionRateChargeCode is required")]
            public string RateCollectionRateChargeCode { get; set; }
        }

        public dbRow dbRow_instance { get; set; }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }

            public string? Owner_Name { get; set; }

            public string? Site_PublicID { get; set; }

            public string? ActionType_PublicID { get; set; }

            public string? UnitType_PublicID { get; set; }

            public string? StockRateCategory_PublicID { get; set; }

            public string? Created_UserPublicID { get; set; }
        }
        public dbRowExtended dbRowExtended_instance { get; set; }
        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "OwnerID", "SiteID", "ActionTypeID", "UnitTypeID",
                "StockRateCategoryID", "RateCollectionRateChargeCode", "WeightBreak", "LengthBreak" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("IsPriority", "Is Priority"),
                        new ChangeTrackerData("ActionTypeID", "Action Type ID"),
                        new ChangeTrackerData("UnitTypeID", "Unit Type ID"),
                        new ChangeTrackerData("StockRateCategoryID", "Stock Rate Category ID"),
                        new ChangeTrackerData("WeightBreak", "Weight Break", true),
                        new ChangeTrackerData("LengthBreak", "Length Break", true),
                        new ChangeTrackerData("RateCollectionRateChargeCode", "Rate Collection Rate Charge Code")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
