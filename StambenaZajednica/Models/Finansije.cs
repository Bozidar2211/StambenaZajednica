namespace StambenaZajednica.Models
{
    public class Finansije
    {
        public int Id { get; set; }  // Primarni ključ
        public required string TipTroska { get; set; }  // Tip troška (npr. investicioni fond, struja, lift...)
        public decimal IznosDuga { get; set; }  // Ukupni iznos duga
        public DateTime DatumDospeca { get; set; }  // Datum dospela za plaćanje
        public required string Status { get; set; }  // Status (plaćeno, nepplaćeno, delimično plaćeno)
        public decimal IznosUplate { get; set; }  // Iznos koji je uplaćen
        public DateTime? DatumUplate { get; set; }  // Datum kada je izvršena uplata (ako postoji)

        // Strani ključ ka stambenoj zajednici
        public int StambenaZajednicaId { get; set; }
        public required StambZajednica StambenaZajednica { get; set; }  // Navigacija ka stambenoj zajednici
    }
}
