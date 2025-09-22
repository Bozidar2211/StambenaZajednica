namespace StambenaZajednica.Helpers
{
    public static class PinHelper
    {
        public static string GenerisiNoviPIN()
        {
            var rnd = new Random();
            return rnd.Next(0, 1000000).ToString("D6"); // uvek 6 cifara
        }
    }
}
