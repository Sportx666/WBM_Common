using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class StockOnHand : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }

            public List<int> ListOwnerID { get; set; }

            public List<int> ListSiteID { get; set; }

            public List<int> ListLocationID { get; set; }

            public List<int> ListStockID { get; set; }

            public DateTime? StockOnHandDate_from { get; set; }

            public DateTime? StockOnHandDate_to { get; set; }

            public int? Quantity_min { get; set; }

            public int? Quantity_max { get; set; }

            public List<int> ListRateCodeID { get; set; }

            public List<int> ListStorageRateCodeFromID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleSite = 2,
                SingleOwner = 3,
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
                else if (SearchMode == SearchModeType.SingleOwner)
                {
                    if (ListOwnerID == null || ListOwnerID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Owner", "OwnerID must be specified."));
                    }

                }
                else if (SearchMode == SearchModeType.SingleSite)
                {
                    if (ListSiteID == null || ListSiteID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Site", "SiteID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

                    if (DataObjectValidator.DateMinMax("StockOnHandDate", StockOnHandDate_from, StockOnHandDate_to, out ListLocalValidationErrors) == false)
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (DataObjectValidator.IntMinMax("Quantity", Quantity_min, Quantity_max, out ListLocalValidationErrors) == false)
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
                sb.Append(@"SELECT StockOnHand.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            Stock.ProductCode AS Stock_ProductCode,
                            Location.PublicLocationID AS Location_PublicLocationID,
                            RateCode.RateCode AS RateCode_RateCode,
                            StorageRateCodeFrom.PublicID AS StorageRateCodeFrom_PublicID,
                            Security_User.PublicID as Created_UserPublicID
                    FROM StockOnHand 
                        LEFT JOIN Site ON StockOnHand.SiteID = Site.ID
                        LEFT JOIN Company ON StockOnHand.CompanyID = Company.ID
                        LEFT JOIN Owner ON StockOnHand.OwnerID = Owner.ID
                        LEFT JOIN Stock ON StockOnHand.StockID = Stock.ID
                        LEFT JOIN Location ON StockOnHand.LocationID = Location.ID
                        LEFT JOIN RateCode ON StockOnHand.RateCodeID = RateCode.ID
                        LEFT JOIN StorageRateCodeFrom ON StockOnHand.StorageRateCodeFromID = StorageRateCodeFrom.ID
                        LEFT JOIN Security_User on StockOnHand.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListLocationID(ListLocationID, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListStockID(ListStockID, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("StockOnHandDate", StockOnHandDate_from, StockOnHandDate_to, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Int_MinMax("Quantity", Quantity_min, Quantity_max, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListRateCodeID(ListRateCodeID, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListStorageRateCodeFromID(ListStorageRateCodeFromID, out generateSQL, out generateListParamater, "StockOnHand."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("StockOnHand",out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                SqlQuery = sb.ToString();
            }

        }

        public dbRow dbRow_instance { get; set; }

        // Used by api/StockOnHandSaveList to insert many rows in one call
        public List<dbRow> ListdbRow_instance { get; set; }
        public class dbRow : dbRowCreatedLast_Base
        {
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Required(ErrorMessage = "StockOnHandDate is required")]
            public DateTime StockOnHandDate { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "LocationID is required")]
            public int LocationID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "StockID is required")]
            public int StockID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Quantity is required")]
            public int Quantity { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "RateCodeID is required")]
            public int RateCodeID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "StorageRateCodeFromID is required")]
            public int StorageRateCodeFromID { get; set; }
            public string PalletNumber { get; set; }
            public int ZoneProcessingID { get; set; }
            public string Status { get; set; }
            public string ConversionCode { get; set; }
            public DateTime? UseByDate { get; set; }
            public string LotNumber { get; set; }
            public Decimal PalletWeight { get; set; }
            public int PalletWidth { get; set; }
            public int PalletDepth { get; set; }
            public int PalletHeight { get; set; }
            public int PalletTypeID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? Location_PublicLocationID { get; set; }
            public string? Stock_ProductCode { get; set; }
            public string? RateCode_RateCode { get; set; }
            public string? StorageRateCodeFrom_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "OwnerID", "StockOnHandDate"
                , "LocationID", "StockID", "Quantity", "RateCodeID", "StorageRateCodeFromID" };
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
                        new ChangeTrackerData("StockOnHandDate", "Stock On Hand Date"),
                        new ChangeTrackerData("LocationID", "Location ID"),
                        new ChangeTrackerData("StockID", "Stock ID"),
                        new ChangeTrackerData("Quantity", "Quantity"),
                        new ChangeTrackerData("RateCodeID", "Rate Code ID"),
                        new ChangeTrackerData("StorageRateCodeFromID", "Storage Rate Code From ID"),
                        new ChangeTrackerData("PalletNumber", "Pallet Number"),
                        new ChangeTrackerData("ZoneProcessingID", "Zone Processing ID"),
                        new ChangeTrackerData("Status", "Status"),
                        new ChangeTrackerData("ConversionCode", "Conversion Code"),
                        new ChangeTrackerData("UseByDate", "Use By Date"),
                        new ChangeTrackerData("LotNumber", "Lot Number"),
                        new ChangeTrackerData("PalletWeight", "Pallet Weight", true),
                        new ChangeTrackerData("PalletWidth", "Pallet Width"),
                        new ChangeTrackerData("PalletDepth", "Pallet Depth"),
                        new ChangeTrackerData("PalletHeight", "Pallet Height"),
                        new ChangeTrackerData("PalletTypeID", "Pallet Type ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
