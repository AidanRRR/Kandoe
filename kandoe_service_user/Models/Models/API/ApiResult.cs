using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Models.API
{
    public class ApiResult<T>
    {
        public ApiResult()
        {
            this.HasErrors = false;
            this.HasWarnings = false;
            this.ErrorMessages = new List<string>();
            this.WarningMessages = new List<string>();
        }

        public bool HasErrors { get; set; }
        public bool HasWarnings { get; set; }

        public List<string> ErrorMessages { get; set; }
        public List<string> WarningMessages { get; set; }

        public T Data { get; set; }
        
    }
}
