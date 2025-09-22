namespace StambenaZajednica.Models
{
    public class RegisterModel
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Ime { get; set; }

        public required string Prezime { get; set; }

        public string? Uloga { get; set; } // "Stanar" ili "Upravnik"

        public int? StanId { get; set; } // Ako se stan dodeljuje odmah
    }
}
