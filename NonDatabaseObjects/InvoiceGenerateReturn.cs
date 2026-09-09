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
    public  class InvoiceGenerateReturn
    {
        public class OwnerResult
        {
            public int OwnerID { get; set; }

            public string InvoiceNumber { get; set; }

            public string ErrorMessage { get; set; }    

            public Boolean IsSuccess { get; set; }

            public OwnerResult(int ownerID, string invoiceNumber, string errorMessage, Boolean isSuccess)
            {
                OwnerID = ownerID;
                InvoiceNumber = invoiceNumber;
                ErrorMessage = errorMessage;
                IsSuccess = isSuccess;
            }

        }

        public List<string> ListErrorMessage { get; set; }

        public List<OwnerResult> ListOwnerResult { get; set; }

    }
}
