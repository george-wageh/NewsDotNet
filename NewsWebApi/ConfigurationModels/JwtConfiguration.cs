namespace NewsWebApi.ConfigurationModels
{
    public class JwtConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
    }
}
