namespace BearStock.Tools.Helpers
{
    public class AuthSettings
    {
        public AuthSettings(string value)
        {
            AuthorizationUrl = value;
        }

        public string AuthorizationUrl { get; set; }
    }
}