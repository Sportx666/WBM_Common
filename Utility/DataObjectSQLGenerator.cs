using Microsoft.Data.SqlClient;
using System.Text;

namespace wbm_common.Utility
{
    public static class DataObjectSQLGenerator
    {
        public static bool Date_FromTo(string DataName, DateTime? date_from, DateTime? date_to, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = new List<SqlParameter>();
            if (date_from.HasValue)
            {
                SQLStatement = " AND " + prefix + DataName + " >= @From" + DataName;
                ListSqlParameter.Add(new SqlParameter("@From" + DataName, date_from.Value));
            }
            if (date_to.HasValue)
            {
                SQLStatement += " AND " + prefix + DataName + " <= @To" + DataName;
                ListSqlParameter.Add(new SqlParameter("@To" + DataName, date_to));
            }
            return true;
        }


        public static bool Decimal_MinMax(string DataName, Decimal? dec_min, Decimal? dec_max, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = new List<SqlParameter>();
            if (dec_min.HasValue)
            {
                SQLStatement = " AND " + prefix + DataName + " >= @Min" + DataName;
                ListSqlParameter.Add(new SqlParameter("@Min" + DataName, dec_min.Value));
            }
            if (dec_max.HasValue)
            {
                SQLStatement += " AND " + prefix + DataName + " <= @Max" + DataName;
                ListSqlParameter.Add(new SqlParameter("@Max" + DataName, dec_max));
            }
            return true;
        }

        public static bool Int_MinMax(string DataName, int? int_min, int? int_max, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = new List<SqlParameter>();
            if (int_min.HasValue)
            {
                SQLStatement = " AND " + prefix + DataName + " >= @Min" + DataName;
                ListSqlParameter.Add(new SqlParameter("@Min" + DataName, int_min.Value));
            }
            if (int_max.HasValue)
            {
                SQLStatement += " AND " + prefix + DataName + " <= @Max" + DataName;
                ListSqlParameter.Add(new SqlParameter("@Max" + DataName, int_max));
            }
            return true;
        }

        public static bool ListActionTypeID(List<int> ListActionTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListActionTypeID == null || ListActionTypeID.Count == 0)
            {
                // No ActionTypeIDs provided
                return false;
            }

            if (ListActionTypeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}ActionTypeID = @ActionTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@ActionTypeID", ListActionTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}ActionTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListActionTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListActionTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListChangeHeaderTableID(List<int> ListChangeHeaderTableID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListChangeHeaderTableID == null || ListChangeHeaderTableID.Count == 0)
            {
                // No ChangeHeaderTableIDs provided
                return false;
            }

            if (ListChangeHeaderTableID.Count == 1)
            {
                SQLStatement = $" AND {prefix}ChangeHeaderTableID = @ChangeHeaderTableID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@ChangeHeaderTableID", ListChangeHeaderTableID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}ChangeHeaderTableID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListChangeHeaderTableID.Count; cnt++)
                {
                    sb.Append(csep + ListChangeHeaderTableID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListCarrierID(List<int> ListCarrierID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListCarrierID == null || ListCarrierID.Count == 0)
            {
                // No CarrierIDs provided
                return false;
            }

            if (ListCarrierID.Count == 1)
            {
                SQLStatement = $" AND {prefix}CarrierID = @CarrierID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@CarrierID", ListCarrierID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}CarrierID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListCarrierID.Count; cnt++)
                {
                    sb.Append(csep + ListCarrierID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListCategoryID(List<int> ListCategoryID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListCategoryID == null || ListCategoryID.Count == 0)
            {
                // No CategoryIDs provided
                return false;
            }

            if (ListCategoryID.Count == 1)
            {
                SQLStatement = $" AND {prefix}CategoryID = @CategoryID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@CategoryID", ListCategoryID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}CategoryID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListCategoryID.Count; cnt++)
                {
                    sb.Append(csep + ListCategoryID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListID(List<int> ListID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListID == null || ListID.Count == 0)
            {
                // No ListIDs provided
                return false;
            }

            if (ListID.Count == 1)
            {
                SQLStatement = $" AND {prefix}ID = @ID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@ID", ListID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}ID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListID.Count; cnt++)
                {
                    sb.Append(csep + ListID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListContainerSizeID(List<int> ListContainerSizeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListContainerSizeID == null || ListContainerSizeID.Count == 0)
            {
                // No ContainerSizeIDs provided
                return false;
            }

            if (ListContainerSizeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}ContainerSizeID = @ContainerSizeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@ContainerSizeID", ListContainerSizeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}ContainerSizeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListContainerSizeID.Count; cnt++)
                {
                    sb.Append(csep + ListContainerSizeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListContainerUnloadTypeID(List<int> ListContainerUnloadTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter)
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListContainerUnloadTypeID == null || ListContainerUnloadTypeID.Count == 0)
            {
                // No ContainerUnloadTypeIDs provided
                return false;
            }

            if (ListContainerUnloadTypeID.Count == 1)
            {
                SQLStatement = " AND ContainerUnloadTypeID = @ContainerUnloadTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@ContainerUnloadTypeID", ListContainerUnloadTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" AND ContainerUnloadTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListContainerUnloadTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListContainerUnloadTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListCompanyID(List<int> ListCompanyID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListCompanyID == null || ListCompanyID.Count == 0)
            {
                // No CompanyIDs provided
                return false;
            }

            if (ListCompanyID.Count == 1)
            {
                SQLStatement = $" AND {prefix}CompanyID = @CompanyID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@CompanyID", ListCompanyID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}CompanyID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListCompanyID.Count; cnt++)
                {
                    sb.Append(csep + ListCompanyID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListCustomerID(List<int> ListCustomerID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListCustomerID == null || ListCustomerID.Count == 0)
            {
                // No CustomerIDs provided
                return false;
            }

            if (ListCustomerID.Count == 1)
            {
                SQLStatement = $" AND {prefix}CustomerID = @CustomerID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@CustomerID", ListCustomerID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}CustomerID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListCustomerID.Count; cnt++)
                {
                    sb.Append(csep + ListCustomerID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListFileTypeID(List<int> ListFileTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListFileTypeID == null || ListFileTypeID.Count == 0)
            {
                // No FileTypeIDs provided
                return false;
            }

            if (ListFileTypeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}FileTypeID = @FileTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@FileTypeID", ListFileTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}FileTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListFileTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListFileTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListInvoiceCycleID(List<int> ListInvoiceCycleID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListInvoiceCycleID == null || ListInvoiceCycleID.Count == 0)
            {
                // No InvoiceCycleIDs provided
                return false;
            }

            if (ListInvoiceCycleID.Count == 1)
            {
                SQLStatement = $" AND {prefix}InvoiceCycleID = @InvoiceCycleID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@InvoiceCycleID", ListInvoiceCycleID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}InvoiceCycleID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListInvoiceCycleID.Count; cnt++)
                {
                    sb.Append(csep + ListInvoiceCycleID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListInvoiceFormatID(List<int> ListInvoiceFormatID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListInvoiceFormatID == null || ListInvoiceFormatID.Count == 0)
            {
                // No InvoiceFormatIDs provided
                return false;
            }

            if (ListInvoiceFormatID.Count == 1)
            {
                SQLStatement = $" AND {prefix}InvoiceFormatID = @InvoiceFormatID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@InvoiceFormatID", ListInvoiceFormatID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}InvoiceFormatID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListInvoiceFormatID.Count; cnt++)
                {
                    sb.Append(csep + ListInvoiceFormatID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }
        public static bool ListInvoiceNumber(List<int> InvoiceNumber, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (InvoiceNumber == null || InvoiceNumber.Count == 0)
            {
                // No InvoiceNumbers provided
                return false;
            }

            if (InvoiceNumber.Count == 1)
            {
                SQLStatement = $" AND {prefix}InvoiceNumber = @InvoiceNumber";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@InvoiceNumber", InvoiceNumber[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}InvoiceNumber IN (");
                string csep = "";
                for (int cnt = 0; cnt < InvoiceNumber.Count; cnt++)
                {
                    sb.Append(csep + InvoiceNumber[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }
        public static bool ListInvoicePrefix(List<string> ListInvoicePrefix, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListInvoicePrefix == null || ListInvoicePrefix.Count == 0)
            {
                // No InvoiceNumbers provided
                return false;
            }

            if (ListInvoicePrefix.Count == 1)
            {
                SQLStatement = $" AND {prefix}InvoicePrefix like @InvoicePrefix ";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@InvoicePrefix", $"{ListInvoicePrefix[0]}%")
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                ListSqlParameter = new List<SqlParameter>();
                sb.Append($" AND ( ");
                List<string> builingstr = new List<string>();
                for (int cnt = 0; cnt < ListInvoicePrefix.Count; cnt++)
                {
                    builingstr.Add($"( {prefix}InvoicePrefix like @InvoicePrefix" + cnt.ToString() + " ) ");
                    ListSqlParameter.Add(new SqlParameter("@InvoicePrefix" + cnt.ToString(), $"{ListInvoicePrefix[cnt]}%"));
                }
                sb.Append(string.Join(" OR ", builingstr));
                sb.Append(" )");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListLocationID(List<int> ListLocationID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListLocationID == null || ListLocationID.Count == 0)
            {
                // No LocationIDs provided
                return false;
            }

            if (ListLocationID.Count == 1)
            {
                SQLStatement = $" AND {prefix}LocationID = @LocationID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@LocationID", ListLocationID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}LocationID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListLocationID.Count; cnt++)
                {
                    sb.Append(csep + ListLocationID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListPalletStorageTypeID(List<int> ListPalletStorageTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListPalletStorageTypeID == null || ListPalletStorageTypeID.Count == 0)
            {
                // No InvoiceCycleIDs provided
                return false;
            }

            if (ListPalletStorageTypeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}PalletStorageTypeID = @PalletStorageTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@PalletStorageTypeID", ListPalletStorageTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}PalletStorageType IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListPalletStorageTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListPalletStorageTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListPaperlessDataTypeID(List<int> ListPaperlessDataTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter)
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListPaperlessDataTypeID == null || ListPaperlessDataTypeID.Count == 0)
            {
                // No InvoiceCycleIDs provided
                return false;
            }

            if (ListPaperlessDataTypeID.Count == 1)
            {
                SQLStatement = " AND PaperlessDataTypeID = @PaperlessDataTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@PaperlessDataTypeID", ListPaperlessDataTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" AND PaperlessDataType IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListPaperlessDataTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListPaperlessDataTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListProcessID(List<int> ListProcessID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListProcessID == null || ListProcessID.Count == 0)
            {
                // No InvoiceCycleIDs provided
                return false;
            }

            if (ListProcessID.Count == 1)
            {
                SQLStatement = $" AND {prefix}ProcessID = @ProcessID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@ProcessID", ListProcessID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}Process IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListProcessID.Count; cnt++)
                {
                    sb.Append(csep + ListProcessID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListPurchaseOrderHeaderID(List<int> ListPurchaseOrderHeaderID, out string SQLStatement, out List<SqlParameter> ListSqlParameter)
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListPurchaseOrderHeaderID == null || ListPurchaseOrderHeaderID.Count == 0)
            {
                // No InvoiceCycleIDs provided
                return false;
            }

            if (ListPurchaseOrderHeaderID.Count == 1)
            {
                SQLStatement = " AND PurchaseOrderHeaderID = @PurchaseOrderHeaderID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@PurchaseOrderHeaderID", ListPurchaseOrderHeaderID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" AND PurchaseOrderHeader IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListPurchaseOrderHeaderID.Count; cnt++)
                {
                    sb.Append(csep + ListPurchaseOrderHeaderID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListNoteTypeID(List<int> ListNoteTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter)
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListNoteTypeID == null || ListNoteTypeID.Count == 0)
            {
                // No NoteTypeIDs provided
                return false;
            }

            if (ListNoteTypeID.Count == 1)
            {
                SQLStatement = " AND NoteTypeID = @NoteTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@NoteTypeID", ListNoteTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" AND NoteTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListNoteTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListNoteTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListOwnerID(List<int> ListOwnerID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListOwnerID == null || ListOwnerID.Count == 0)
            {
                // No OwnerIDs provided
                return false;
            }

            if (ListOwnerID.Count == 1)
            {
                SQLStatement = $" AND {prefix}OwnerID = @OwnerID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@OwnerID", ListOwnerID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}OwnerID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListOwnerID.Count; cnt++)
                {
                    sb.Append(csep + ListOwnerID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListRateID(List<int> ListRateID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListRateID == null || ListRateID.Count == 0)
            {
                // No RateIDs provided
                return false;
            }

            if (ListRateID.Count == 1)
            {
                SQLStatement = $" AND {prefix}RateID = @RateID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@RateID", ListRateID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}RateID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListRateID.Count; cnt++)
                {
                    sb.Append(csep + ListRateID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListRateCodeID(List<int> ListRateCodeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListRateCodeID == null || ListRateCodeID.Count == 0)
            {
                // No RateCodeIDs provided
                return false;
            }

            if (ListRateCodeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}RateCodeID = @RateCodeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@RateCodeID", ListRateCodeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}RateCodeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListRateCodeID.Count; cnt++)
                {
                    sb.Append(csep + ListRateCodeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListNewRateCodeID(List<int> ListNewRateCodeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListNewRateCodeID == null || ListNewRateCodeID.Count == 0)
            {
                // No NewRateCodeIDs provided
                return false;
            }

            if (ListNewRateCodeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}NewRateCodeID = @NewRateCodeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@NewRateCodeID", ListNewRateCodeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}NewRateCodeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListNewRateCodeID.Count; cnt++)
                {
                    sb.Append(csep + ListNewRateCodeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListOrderHeaderID(List<int> ListOrderHeaderID, out string SQLStatement, out List<SqlParameter> ListSqlParameter)
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListOrderHeaderID == null || ListOrderHeaderID.Count == 0)
            {
                // No InvoiceCycleIDs provided
                return false;
            }

            if (ListOrderHeaderID.Count == 1)
            {
                SQLStatement = " AND OrderHeaderID = @OrderHeaderID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@OrderHeaderID", ListOrderHeaderID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" AND OrderHeader IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListOrderHeaderID.Count; cnt++)
                {
                    sb.Append(csep + ListOrderHeaderID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListRateTypeID(List<int> ListRateTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter)
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListRateTypeID == null || ListRateTypeID.Count == 0)
            {
                // No RateTypeIDs provided
                return false;
            }

            if (ListRateTypeID.Count == 1)
            {
                SQLStatement = " AND RateTypeID = @RateTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@RateTypeID", ListRateTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" AND RateTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListRateTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListRateTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListRateFunctionID(List<int> ListRateFunctionID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListRateFunctionID == null || ListRateFunctionID.Count == 0)
            {
                // No RateFunctionIDs provided
                return false;
            }

            if (ListRateFunctionID.Count == 1)
            {
                SQLStatement = $" AND {prefix}RateFunctionID = @RateFunctionID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@RateFunctionID", ListRateFunctionID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}RateFunctionID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListRateFunctionID.Count; cnt++)
                {
                    sb.Append(csep + ListRateFunctionID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListStatusID(List<int> ListStatusID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListStatusID == null || ListStatusID.Count == 0)
            {
                // No StatusIDs provided
                return false;
            }

            if (ListStatusID.Count == 1)
            {
                SQLStatement = $" AND {prefix}StatusID = @StatusID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@StatusID", ListStatusID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}StatusID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListStatusID.Count; cnt++)
                {
                    sb.Append(csep + ListStatusID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListSiteID(List<int> ListSiteID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListSiteID == null || ListSiteID.Count == 0)
            {
                // No SiteIDs provided
                return false;
            }

            if (ListSiteID.Count == 1)
            {
                SQLStatement = $" AND {prefix}SiteID = @SiteID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@SiteID", ListSiteID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}SiteID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListSiteID.Count; cnt++)
                {
                    sb.Append(csep + ListSiteID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListStockID(List<int> ListStockID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListStockID == null || ListStockID.Count == 0)
            {
                // No StockIDs provided
                return false;
            }

            if (ListStockID.Count == 1)
            {
                SQLStatement = $" AND {prefix}StockID = @StockID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@StockID", ListStockID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}StockID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListStockID.Count; cnt++)
                {
                    sb.Append(csep + ListStockID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListStockRateCategoryID(List<int> ListStockRateCategoryID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListStockRateCategoryID == null || ListStockRateCategoryID.Count == 0)
            {
                // No StockRateCategoryIDs provided
                return false;
            }

            if (ListStockRateCategoryID.Count == 1)
            {
                SQLStatement = $" AND {prefix}StockRateCategoryID = @StockRateCategoryID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@StockRateCategoryID", ListStockRateCategoryID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}StockRateCategoryID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListStockRateCategoryID.Count; cnt++)
                {
                    sb.Append(csep + ListStockRateCategoryID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListStorageRateCodeFromID(List<int> ListStorageRateCodeFromID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListStorageRateCodeFromID == null || ListStorageRateCodeFromID.Count == 0)
            {
                // No StorageRateCodeFromIDs provided
                return false;
            }

            if (ListStorageRateCodeFromID.Count == 1)
            {
                SQLStatement = $" AND {prefix}StorageRateCodeFromID = @StorageRateCodeFromID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@StorageRateCodeFromID", ListStorageRateCodeFromID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}StorageRateCodeFromID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListStorageRateCodeFromID.Count; cnt++)
                {
                    sb.Append(csep + ListStorageRateCodeFromID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListSourceID(List<int> ListSourceID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListSourceID == null || ListSourceID.Count == 0)
            {
                // No SourceIDs provided
                return false;
            }

            if (ListSourceID.Count == 1)
            {
                SQLStatement = $" AND {prefix}SourceID = @SourceID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@SourceID", ListSourceID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}SourceID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListSourceID.Count; cnt++)
                {
                    sb.Append(csep + ListSourceID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListSystemID(List<int> ListSystemID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListSystemID == null || ListSystemID.Count == 0)
            {
                // No SystemIDs provided
                return false;
            }

            if (ListSystemID.Count == 1)
            {
                SQLStatement = $" AND {prefix}SystemID = @SystemID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@SystemID", ListSystemID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}SystemID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListSystemID.Count; cnt++)
                {
                    sb.Append(csep + ListSystemID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListSiteSystemID(List<int> ListSiteSystemID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListSiteSystemID == null || ListSiteSystemID.Count == 0)
            {
                // No SiteSystemIDs provided
                return false;
            }

            if (ListSiteSystemID.Count == 1)
            {
                SQLStatement = $" AND {prefix}SiteSystemID = @SiteSystemID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@SiteSystemID", ListSiteSystemID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}SiteSystemID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListSiteSystemID.Count; cnt++)
                {
                    sb.Append(csep + ListSiteSystemID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListTransactionExceptionTypeID(List<int> ListTransactionExceptionTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListTransactionExceptionTypeID == null || ListTransactionExceptionTypeID.Count == 0)
            {
                // No TransactionExceptionTypeIDs provided
                return false;
            }

            if (ListTransactionExceptionTypeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}TransactionExceptionTypeID = @TransactionExceptionTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@TransactionExceptionTypeID", ListTransactionExceptionTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}TransactionExceptionTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListTransactionExceptionTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListTransactionExceptionTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListTransactionHeaderID(List<int> ListTransactionHeaderID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListTransactionHeaderID == null || ListTransactionHeaderID.Count == 0)
            {
                // No TransactionHeaderIDs provided
                return false;
            }

            if (ListTransactionHeaderID.Count == 1)
            {
                SQLStatement = $" AND {prefix}TransactionHeaderID = @TransactionHeaderID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@TransactionHeaderID", ListTransactionHeaderID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}TransactionHeaderID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListTransactionHeaderID.Count; cnt++)
                {
                    sb.Append(csep + ListTransactionHeaderID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListTransactionDetailID(List<int> ListTransactionDetailID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListTransactionDetailID == null || ListTransactionDetailID.Count == 0)
            {
                // No TransactionDetailIDs provided
                return false;
            }

            if (ListTransactionDetailID.Count == 1)
            {
                SQLStatement = $" AND {prefix}TransactionDetailID = @TransactionDetailID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@TransactionDetailID", ListTransactionDetailID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}TransactionDetailID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListTransactionDetailID.Count; cnt++)
                {
                    sb.Append(csep + ListTransactionDetailID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListTransactionSourceID(List<int> ListTransactionSourceID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListTransactionSourceID == null || ListTransactionSourceID.Count == 0)
            {
                // No TransactionSourceIDs provided
                return false;
            }

            if (ListTransactionSourceID.Count == 1)
            {
                SQLStatement = $" AND {prefix}TransactionSourceID = @TransactionSourceID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@TransactionSourceID", ListTransactionSourceID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}TransactionSourceID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListTransactionSourceID.Count; cnt++)
                {
                    sb.Append(csep + ListTransactionSourceID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListTransactionTypeID(List<int> ListTransactionTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListTransactionTypeID == null || ListTransactionTypeID.Count == 0)
            {
                // No TransactionTypeIDs provided
                return false;
            }

            if (ListTransactionTypeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}TransactionTypeID = @TransactionTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@TransactionTypeID", ListTransactionTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}TransactionTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListTransactionTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListTransactionTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }

        public static bool ListTriggerID(List<int> ListTriggerID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListTriggerID == null || ListTriggerID.Count == 0)
            {
                // No TriggerIDs provided
                return false;
            }

            if (ListTriggerID.Count == 1)
            {
                SQLStatement = $" AND {prefix}TriggerID = @TriggerID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@TriggerID", ListTriggerID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}TriggerID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListTriggerID.Count; cnt++)
                {
                    sb.Append(csep + ListTriggerID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


        public static bool ListUnitTypeID(List<int> ListUnitTypeID, out string SQLStatement, out List<SqlParameter> ListSqlParameter, string prefix = "")
        {
            SQLStatement = null;
            ListSqlParameter = null;
            if (ListUnitTypeID == null || ListUnitTypeID.Count == 0)
            {
                // No UnitTypeIDs provided
                return false;
            }

            if (ListUnitTypeID.Count == 1)
            {
                SQLStatement = $" AND {prefix}UnitTypeID = @UnitTypeID";

                ListSqlParameter = new List<SqlParameter>
                {
                    new SqlParameter("@UnitTypeID", ListUnitTypeID[0])
                };
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($" AND {prefix}UnitTypeID IN (");
                string csep = "";
                for (int cnt = 0; cnt < ListUnitTypeID.Count; cnt++)
                {
                    sb.Append(csep + ListUnitTypeID[cnt]);
                    csep = ",";
                }
                sb.Append(")");

                SQLStatement = sb.ToString();
            }

            return true;
        }


    }
}
