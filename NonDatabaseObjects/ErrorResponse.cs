using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wbm_common.NonDatabaseObjects
{
    public class ErrorResponse
    {
        public string APITransctionReference { get; set; }

        public ErrorList ErrorList { get; set; }
    }

    public class ErrorList
    {
        public List<string> Errors { get; set; }
    }

    public class ErrorMessage
    {
        public string Error { get; set; }
        public bool FiveHundred = false;
    }
}
