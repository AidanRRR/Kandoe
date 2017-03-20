namespace API.Authentication
{
    public class VerifyResult
    {
        public bool Succes { get; set; }

        public class DecodedToken {
            public string Name { get; set; }
            public int lat { get; set; }
            public int Exp { get; set; }
        }

        public DecodedToken Decoded { get; set; }
        public bool Success { get; internal set; }
    }
}