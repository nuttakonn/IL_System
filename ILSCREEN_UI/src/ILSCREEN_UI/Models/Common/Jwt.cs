namespace ILSCREEN_UI.Models
{
    public class Jwt
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? IssuerReceive { get; set; }
        public string? IssuerWithdraw { get; set; }
        public string? ExpireMinutes { get; set; }
        public string? Role { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool Enable { get; set; }
        public bool Authen { get; set; }
        public string? ActionIgnore { get; set; }
    }
}
