namespace StambenaZajednica.Models
{
    public class StambZajednica
    {
        public int Id { get; set; }  // Primarni ključ
        public required string Naziv { get; set; }  // Naziv stambene zajednice
        public required string Adresa { get; set; }  // Adresa (ulica, broj, mesto/gas)
        public required string BrojZgrade { get; set; }  // Broj zgrade u okviru zajednice

        // Relacija 1-n sa Stanovima
        public ICollection<Stan> Stanovi { get; set; } = new List<Stan>();

        // Relacija 1-n sa Finansijama
        public ICollection<Finansije> Finansije { get; set; } = new List<Finansije>();
    }
}
