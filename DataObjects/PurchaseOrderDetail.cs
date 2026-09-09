using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class PurchaseOrderDetail
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListPurchaseOrderHeaderID { get; set; }

            public List<int> ListCompanyID { get; set; }
            public List<int> ListSiteID { get; set; }
            public List<int> ListOwnerID { get; set; }

            public int LineNumber { get; set; }

            public string ProductCode { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SinglePurchaseOrderHeaderID = 2,
                AllRecords = 3,
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
                else if (SearchMode == SearchModeType.SinglePurchaseOrderHeaderID)
                {
                    if (ListPurchaseOrderHeaderID == null || ListPurchaseOrderHeaderID.Count != 1 || ListPurchaseOrderHeaderID[0] <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("PurchaseOrderHeaderID", "Single Purchase Order Header ID must be specified."));
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
                sb.Append("SELECT * FROM PurchaseOrderDetail WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListPurchaseOrderHeaderID(ListPurchaseOrderHeaderID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }   

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(ProductCode))
                {
                    sb.Append(" AND ProductCode like '%' + @ProductCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@ProductCode", ProductCode));
                }

                if (BaseGenerateSqlQuery("PurchaseOrderDetail", out generateSQL, out generateListParamater))
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
            public int PurchaseOrderHeaderID { get; set; }

            public int CompanyID { get; set; }
            public int SiteID { get; set; }
            public int OwnerID { get; set; }
            public int LineNumber { get; set; }
            public int StockID { get; set; }
            public int QuantityOrdered { get; set; }

            public int QuantityReceived { get; set; }

            public int PalletStorageRateCodeID { get; set; }

            [NotMapped]
            public string Company_ShortName { get; set; }

            [NotMapped]
            public string Owner_Name { get; set; }

            [NotMapped]
            public string Site_PublicID { get; set; }

            [NotMapped]
            public string Stock_ProductCode { get; set; }

            [NotMapped]
            public string PalletStorageType_PublicID { get; set; }

        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
