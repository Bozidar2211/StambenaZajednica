namespace StambenaZajednica.Models
{
    public enum TipTroska
    {
        InvesticioniFond,
        RedovnoOdrzavanje,
        Ciscenje,
        Struja,
        Lift,
        Ostalo
    }

    public class Racun
    {
        public int Id { get; set; }

        public DateTime Datum { get; set; } // datum kreiranja
        public DateTime DatumDospeca { get; set; } // obavezan

        public decimal Iznos { get; set; } // obavezan
        public string? Opis { get; set; }

        public TipTroska Tip { get; set; } // obavezan

        public bool JePlaceno { get; set; } = false;

        // Račun za stan (opciono)
        public int? StanId { get; set; }
        public Stan? Stan { get; set; }

        // Račun za zajednicu (obavezno)
        public int StambZajednicaId { get; set; }
        public StambZajednica? StambZajednica { get; set; }

        public ICollection<Uplata>? Uplate { get; set; }

        // Dinamički izračunati podaci
        public decimal UkupnoUplaceno => Uplate?.Sum(u => u.Iznos) ?? 0;

        public decimal Visak => UkupnoUplaceno > Iznos ? UkupnoUplaceno - Iznos : 0;

        public decimal PreostaliDug => UkupnoUplaceno >= Iznos ? 0 : Iznos - UkupnoUplaceno;

        public string Status
        {
            get
            {
                if (UkupnoUplaceno == 0) return "Neplaćeno";
                if (UkupnoUplaceno < Iznos) return "Delimično plaćeno";
                if (UkupnoUplaceno == Iznos) return "Plaćeno";
                return "Preplaćeno";
            }
        }
    }
}
