using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
    public class VerifyResult
    {
        public bool Success { get; set; }

        public class DecodedToken
        {
            public string Name { get; set; }
            public int Iat { get; set; }
            public int Exp { get; set; }
        }

        public DecodedToken Decoded { get; set; }

    }
}
