namespace ILSCREEN_UI.Models
{
    public class TokenModel
    {
        public string token { get; set; }
    }

    public class RequestTokenModel
    {
        public string userName { set; get; } = "";
        public string password { set; get; } = "";
        public string role { set; get; } = "";
        public string issuer { set; get; } = "";
        public int expiresMinutes { set; get; } = 2;
    }
}
