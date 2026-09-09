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
    public class InvoiceGenerateHolder
    {
        public int CompanyID { get; set; }

        public int SiteID { get; set; }

        public int InvoiceCycleID { get; set; }

        public DateTime InvoiceDate { get; set; }

        public List<int> ListOwnerID { get; set; }

        public string CalledFrom { get; set; }

        public List<Tuple<string, string>> ValidationErrors { get; set; }

        public System.Boolean IsValid()
        {
            ValidationErrors = new List<Tuple<string, string>>();

            if (CompanyID <= 0)
            {
                ValidationErrors.Add(new Tuple<string, string>("CompanyID", "CompanyID must be specified."));
            }

            if (SiteID <= 0)
            {
                ValidationErrors.Add(new Tuple<string, string>("SiteID", "SiteID must be specified."));
            }

            if (InvoiceCycleID <= 0)
            {
                ValidationErrors.Add(new Tuple<string, string>("InvoiceCycleID", "InvoiceCycleID must be specified."));
            }

            if (InvoiceDate != InvoiceDate.Date)
            {
                ValidationErrors.Add(new Tuple<string, string>("InvoiceDate", "InvoiceDate must be a date without time."));
            }

            if (ListOwnerID == null || ListOwnerID.Count == 0)
            {
                ValidationErrors.Add(new Tuple<string, string>("ListOwnerID", "At least one OwnerID must be specified."));
            }

            if (string.IsNullOrEmpty(CalledFrom))
            {
                ValidationErrors.Add(new Tuple<string, string>("CalledFrom", "CalledFrom must be specified."));
            }

            return ValidationErrors.Count == 0;

        }
    }
}
