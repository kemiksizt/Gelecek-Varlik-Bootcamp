using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Extension
{
    public static class Extensions
    {
        // Enum sınıfında oluşturulan değişkenlerin tipinin alındığı yer(CurrencyTypeDollar)
        public static string GetCurrencies(this Enum currencyType)
        {
            var result = currencyType.GetType().GetMember(currencyType.ToString()).First().GetCustomAttributes<DisplayAttribute>().First().Name;
            return result.ToString();
        }

        // Extension : Türk Lirası --> Dolar 
        public static string toUSD(this double variable)
        {
            return variable / 14.5 + "$";
        }

        // Extension : Türk Lirası --> Euro
        public static string toEuro(this double variable)
        {
            return variable / 16.0 + "€";
        }
    }
}
