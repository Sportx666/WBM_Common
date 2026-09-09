using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wbm_common.NonDatabaseObjects
{
    public class AuthenticationResponse
    {
        public string? APITransactionReference { get; set; }

        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
        public string AccessList { get; set; }
    }
}
