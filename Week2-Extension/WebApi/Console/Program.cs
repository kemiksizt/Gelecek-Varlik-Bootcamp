using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double amount = 15;
            // Türk Lirası --> Dolar(Extension)
            Console.WriteLine(Extension.Extensions.toUSD(amount));
            // Para biriminin tipini bastıran kısım(Enum GetType)
            Console.WriteLine(Extension.Extensions.GetCurrencies(Extension.Enum.CurrencyTypeDollar));
        }
    }
}
