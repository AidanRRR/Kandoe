using System.Collections.Generic;

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

    public class EnumerableApiResult<T> {
        public EnumerableApiResult()
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
        public IEnumerable<T> Data { get; set; }
    }
}