namespace StambenaZajednica.Models
{
    public class Uplata
    {
        public int Id { get; set; }

        public DateTime DatumUplate { get; set; }
        public decimal Iznos { get; set; }

        public int RacunId { get; set; }
        public Racun ? Racun { get; set; }

        public string? StanarId { get; set; }
        public ApplicationUser? Stanar { get; set; }
    }
}
