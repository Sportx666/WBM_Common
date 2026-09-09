using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbm_common.DataObjects;

namespace wbm_common.NonDatabaseObjects
{
    public class PreInvoiceCloseCheck
    {
        public int CompanyID { get; set; }

        public int InvoiceCycleID { get; set; }

        public DateTime NextPeriodEnding { get; set; }

        public string CalledFrom { get; set; }

        public class CheckResult
        {
            public string Category { get; set; }

            public string Status { get; set; }

            public string Description { get; set; }

            public CheckResult()
            {
                Category = string.Empty;
                Status = string.Empty;
                Description = string.Empty;
            }

            public CheckResult(string category, string status, string description)
            {
                Category = category;
                Status = status;
                Description = description;
            }
        }

        public List<CheckResult> ListResult { get; set; }

        public string ErrorMessage { get; set; }

        public PreInvoiceCloseCheck()
        {
            this.ListResult = new List<CheckResult>();
        }

        public Boolean HasCritical
        {
            get
            {
                if (ListResult == null)
                {
                    return false;
                }
                return ListResult.Any(r => r.Status.Equals("Critical", StringComparison.OrdinalIgnoreCase));

            }
        }
    }
}
