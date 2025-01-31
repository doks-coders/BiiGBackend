namespace BiiGBackend.Models.Requests
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }
        public string? AccountType { get; set; }
    }


    public class RegisterUserRequestComplete
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Verify { get; set; }
        public string? AccountType { get; set; }
    }
}
