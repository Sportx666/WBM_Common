using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbm_common.DataObjects;
using wbm_common.Utility;
using static wbm_common.NonDatabaseObjects.Transaction_Costing;

namespace wbm_common.NonDatabaseObjects
{
    public  class TransactionHolder
    {
        public class Search : SearchCreatedLast_Base
        {
            public int TransactionHeaderID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                TransactionHeaderID = 1
            }

            public SearchModeType SearchMode { get; set; } = SearchModeType.Undefined;

            public System.Boolean IsValid()
            {
                ValidationErrors = new List<Tuple<string, string>>();

                System.Boolean rtnVal = true; // assume valid

                if (SearchMode == SearchModeType.TransactionHeaderID)
                {
                    if (TransactionHeaderID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("TransactionHeaderID", "Single TransactionHeaderID must be specified."));
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
                sb.Append(@"SELECT TransactionException.*,
                            TransactionHeader.Description AS TransactionHeader_Description,
                            TransactionDetail.FullDescription AS TransactionDetail_FullDescription,
                            TransactionExceptionType.PublicID AS TransactionExceptionType_PublicID
                    FROM TransactionException 
                        LEFT JOIN TransactionHeader ON TransactionException.TransactionHeaderID = TransactionHeader.ID
                        LEFT JOIN TransactionDetail ON TransactionException.TransactionDetailID = TransactionDetail.ID
                        LEFT JOIN TransactionExceptionType ON TransactionException.TransactionExceptionTypeID = TransactionExceptionType.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (BaseGenerateSqlQuery("TransactionException", out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                SqlQuery = sb.ToString();
            }


        }

        #region Transaction rows
        public TransactionHeader.dbRow TransactionHeader_Row { get; set; }
        public List<TransactionDetail.dbRow> ListTransactionDetail { get; set; }

        #endregion

        public string PaperlessID { get; set; }


        #region Change Log
        public bool ShouldSerializeListChangeHeader()
        {
            if (DontSerialiseExtraData)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<ChangeHeader.dbRow> ListChangeHeader { get; set; }

        public bool ShouldSerializeListChangeDetail()
        {
            if (DontSerialiseExtraData)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<ChangeDetail.dbRow> ListChangeDetail { get; set; }

        #endregion

        #region Related transaction rows

        public List<TransactionException.dbRow> ListTransactionException { get; set; }

        public List<TransactionNotes.dbRow> ListTransactionNotes { get; set; }

        #endregion

        #region WBM releated transaction rows

        public OrderHeader.dbRow OrderHeader_Row { get; set; }

        public PurchaseOrderHeader.dbRow PurchaseOrderHeader_Row { get; set; }

        public PaperlessTransactionXref.dbRow PaperlessTransactionXref_Row { get; set; }

        //public InvoiceHHeader.dbRow ihRow { get; set; }
        public List<ToDo.dbRow> ListToDo { get; set; }

        #endregion


        public string CalledFrom { get; set; }

        public DateTime ChangedTime { get; set; }
        public Boolean SavedOK { get; set; }
        public DateTime SavedLastAmendDateTime { get; set; }

        public Boolean DeleteTransactionFlag { get; set; }

        public Boolean DontSerialiseExtraData { get; set; }

        public List<string> ListErrorMessage { get; set; }

    }
}
