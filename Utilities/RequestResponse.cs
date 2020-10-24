using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class RequestResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

        public RequestResponse()
        {
            this.Status = true;
            this.Message = Messages.TXN_REALIZADA_OK;
        }

        public RequestResponse(string message)
        {
            this.Status = false;
            this.Message = message;
        }
    }
}
