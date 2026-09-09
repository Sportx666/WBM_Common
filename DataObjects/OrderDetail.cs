using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class OrderDetail
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListOrderHeaderID { get; set; }

            public List<int> ListCompanyID { get; set; }
            public List<int> ListSiteID { get; set; }
            public List<int> ListOwnerID { get; set; }

            public string ProductCode { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleOrderHeaderID = 2,
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
                else if (SearchMode == SearchModeType.SingleOrderHeaderID)
                {
                    if (ListOrderHeaderID == null || ListOrderHeaderID.Count !=1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("OrderHeaderID", "One Order Header ID must be specified."));
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
                sb.Append(@"SELECT OrderDetail.*,
                                 Security_User.PublicID as Created_UserPublicID
                            FROM OrderDetail 
                                 LEFT JOIN Security_User on Stock.CreatedUserID = Security_User.ID
                            WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListOrderHeaderID(ListOrderHeaderID, out generateSQL, out generateListParamater))
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

                if (BaseGenerateSqlQuery("OrderDetail", out generateSQL, out generateListParamater))
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
            public int OrderHeaderID { get; set; }

            public int CompanyID { get; set; }
            public int SiteID { get; set; }
            public int OwnerID { get; set; }
            public int StockID { get; set; }
            public int Quantity { get; set; }


        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }

            public string? Owner_Name { get; set; }

            public string? Site_PublicID { get; set; }

            public string? Stock_ProductCode { get; set; }

            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
