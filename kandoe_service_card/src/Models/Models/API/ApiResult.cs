using System.Collections.Generic;

namespace Models.Models.API
{
    public class ApiResult<T>
    {
        public ApiResult()
        {
            HasErrors = false;
            HasWarnings = false;
            ErrorMessages = new List<string>();
            WarningMessages = new List<string>();
        }

        public bool HasErrors { get; set; }
        public bool HasWarnings { get; set; }

        public List<string> ErrorMessages { get; set; }
        public List<string> WarningMessages { get; set; }

        public T Data { get; set; }

    }
}
