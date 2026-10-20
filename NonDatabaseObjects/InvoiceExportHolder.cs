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
    public class InvoiceExportHolder
    {
        public int CompanyID { get; set; }

        public List<InvoiceHeader.dbRow> ListInvoiceHeader { get; set; }

        public string CalledFrom { get; set; }

        public List<Tuple<string, string>> ValidationErrors { get; set; }

        public System.Boolean IsValid()
        {
            ValidationErrors = new List<Tuple<string, string>>();

            if (CompanyID <= 0)
            {
                ValidationErrors.Add(new Tuple<string, string>("CompanyID", "CompanyID must be specified."));
            }

            if (ListInvoiceHeader == null || ListInvoiceHeader.Count == 0)
            {
                ValidationErrors.Add(new Tuple<string, string>("ListInvoiceHeaderID", "At least one Invoice must be specified."));
            }

            if (string.IsNullOrEmpty(CalledFrom))
            {
                ValidationErrors.Add(new Tuple<string, string>("CalledFrom", "CalledFrom must be specified."));
            }

            return ValidationErrors.Count == 0;

        }
    }
}
