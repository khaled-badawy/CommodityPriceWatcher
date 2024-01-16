namespace Commodity_Prices_Watcher.BL
{
    public class ForgetPasswordModel
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
    }
}
