using StambenaZajednica.Models;

namespace StambenaZajednica.Models
{
    public class Stan
    {
        public int Id { get; set; }
        public string? BrojStana { get; set; }
        public int Kvadratura { get; set; }

        public int ZajednicaId { get; set; }
        public StambZajednica? Zajednica { get; set; }

        public string? StanarId { get; set; } // Identity korisnik
        public ApplicationUser? Stanar { get; set; }

        // PIN od 6 cifara
        public string? Pin { get; set; }

        // Broj neuspešnih pokušaja
        public int PinAttempts { get; set; } = 0;

        // Zaključavanje pristupa finansijama
        public bool FinansijeZakljucane { get; set; } = false;

        // Kada je nalog otključan
        public DateTime? DatumOtkljucavanjaFinansija { get; set; }

        // Da li je stanar obavešten o otključavanju
        public bool ObavestenONovomPINu { get; set; } = false;

        public ICollection<Racun>? Racuni { get; set; }
    }
}