namespace WebApi.Model
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
