namespace BlogAPI
{
    public class Configuration
    {
        //Token - JWT (djot) - Json Web Token
        public static string JwtKey = "ChaveLundiAPI20231227TesteRonanNetAPI";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "curso_api_AIUNGOANGOANSONgoagasoisgaoin";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration 
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }

    }
}
