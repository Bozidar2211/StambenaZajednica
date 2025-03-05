namespace StambenaZajednica.Models
{
    public class Stan
    {
        public int Id { get; set; }  // Primarni ključ
        public required string BrojStana { get; set; }  // Broj stana
        public required string Pin { get; set; }  // PIn kod za pristup finansijama (6 cifara)
        public bool DaLiJeZakljucan { get; set; }  // Da li je nalog stanara zaključan zbog previše pokušaja PINA

        // Strani ključ prema stambenoj zajednici
        public int StambenaZajednicaId { get; set; }
        public required StambZajednica StambenaZajednica { get; set; }  // Navigacija ka stambenoj zajednici
    }
}
