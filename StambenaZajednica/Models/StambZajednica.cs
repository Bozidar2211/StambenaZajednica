using StambenaZajednica.Models;
namespace StambenaZajednica.Models
{
    public class StambZajednica
    {
        public int Id { get; set; }
        public string? Naziv { get; set; }
        public string? Adresa { get; set; }
        public string? BrojZgrade { get; set; }


        public ICollection<Stan>? Stanovi { get; set; }
    }
}
