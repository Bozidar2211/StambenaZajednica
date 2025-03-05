namespace StambenaZajednica.Models
{
    public class StambZajednica
    {
        public int Id { get; set; }  // Primarni ključ
        public required string Naziv { get; set; }  // Naziv stambene zajednice
        public required string Adresa { get; set; }  // Adresa (ulica, broj, mesto/gas)
        public required string BrojZgrade { get; set; }  // Broj zgrade u okviru zajednice
    }
}
