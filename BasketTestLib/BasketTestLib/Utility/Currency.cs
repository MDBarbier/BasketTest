using System.Globalization;

namespace BasketTestLib.Utility
{
    public static class Currency
    {
        public static string GetCurrencySymbol()
        {
            return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
        }
    }
}
