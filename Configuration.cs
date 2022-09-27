namespace Blog;

public static class Configuration
{
    public static string JwtKey { get; set; } = "ZmVkYWY3ZDg4NjNiNDhlÇTk3jky0DdkDkyYjcwOGç=";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "curso_api_etmçalms]z~xc?:z==";
    public static SmtpConfiguration Smtp = new SmtpConfiguration();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
