namespace StambenaZajednica.Models
{
    public class LoginModel
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public int? Pin { get; set; } // PIN dodat kao dodatna autentifikacija

        public bool RememberMe { get; set; }
    }
}
