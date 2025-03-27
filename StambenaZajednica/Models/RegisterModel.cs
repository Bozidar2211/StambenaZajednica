namespace StambenaZajednica.Models
{
    public class RegisterModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Pin { get; set; }  // Pin koji se generiše nakon prijave
    }

}
