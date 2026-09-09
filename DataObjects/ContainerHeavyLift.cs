using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ContainerHeavyLift : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }

            public Decimal? MinWeight_min { get; set; }
            public Decimal? MinWeight_max { get; set; }
            public Decimal? MinLength_min { get; set; }
            public Decimal? MinLength_max { get; set; }

            public Decimal? Percentage_min { get; set; }
            public Decimal? Percentage_max { get; set; }

            public string RateCollectionRateChargeCode { get; set; }  

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

                    if (!DataObjectValidator.DecimalMinMax("MinWeight", MinWeight_min, MinWeight_max, out ListLocalValidationErrors))
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (!DataObjectValidator.DecimalMinMax("MinLength", MinLength_min, MinLength_max, out ListLocalValidationErrors))
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (!DataObjectValidator.DecimalMinMax("Percentage", Percentage_min, Percentage_max, out ListLocalValidationErrors))
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
                sb.Append(@"SELECT ContainerHeavylift.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            RateCode.Description AS RateCode_RateChargeCode,
                            Security_User.PublicID as Created_UserPublicID
                    FROM ContainerHeavylift
                        LEFT JOIN Site ON ContainerHeavyLift.SiteID = Site.ID
                        LEFT JOIN Company ON ContainerHeavyLift.CompanyID = Company.ID
                        LEFT JOIN Owner ON ContainerHeavyLift.OwnerID = Owner.ID
                        LEFT JOIN RateCode ON ContainerHeavyLift.RateCollectionRateChargeCode = RateCode.RateCode
                        LEFT JOIN Security_User on ContainerHeavyLift.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "ContainerHeavylift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "ContainerHeavylift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "ContainerHeavylift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("MinWeight", MinWeight_min, MinWeight_max, out generateSQL, out generateListParamater, "ContainerHeavylift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("MinLength", MinLength_min, MinLength_max, out generateSQL, out generateListParamater, "ContainerHeavylift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("Percentage", Percentage_min, Percentage_max, out generateSQL, out generateListParamater, "ContainerHeavylift."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(RateCollectionRateChargeCode))
                {
                    sb.Append(" AND ContainerHeavylift.RateCollectionRateChargeCode like '%' + @RateCollectionRateChargeCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@RateCollectionRateChargeCode", RateCollectionRateChargeCode));
                }


                if (BaseGenerateSqlQuery("ContainerHeavylift",out generateSQL, out generateListParamater))
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
            [Required(ErrorMessage = "MinWeight is required")]
            public Decimal MinWeight { get; set; }
            [Required(ErrorMessage = "MinLength is required")]
            public Decimal MinLength { get; set; }
            [Required(ErrorMessage = "Percentage is required")]
            public Decimal Percentage { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "RateCollectionRateChargeCode is required")]
            public string RateCollectionRateChargeCode { get; set; }
        }

        public dbRow dbRow_instance { get; set; }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? RateCode_RateChargeCode { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }
        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "OwnerID", "MinWeight", "MinLength", "Percentage", "RateCollectionRateChargeCode" };
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
                        new ChangeTrackerData("MinWeight", "Min Weight", true),
                        new ChangeTrackerData("MinLength", "Min Length", true),
                        new ChangeTrackerData("Percentage", "Percentage", true),
                        new ChangeTrackerData("RateCollectionRateChargeCode", "Rate Collection Rate Charge Code")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
