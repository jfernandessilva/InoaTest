using RestApiClient.Classes;
using System;
using System.Threading;

namespace InoaTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Error: Parameters not infomed: Ticker Symbol, Reference Sell or Buy Price");
                return;
            }

            decimal _vlBuyPrice;
            decimal _vlSellPrice;
            Decimal.TryParse(args[1], out _vlSellPrice);
            Decimal.TryParse(args[2], out _vlBuyPrice);

            if (_vlSellPrice == 0 || _vlBuyPrice == 0)
            {
                Console.WriteLine("Error: One or more reference price equal 0");
                return;
            }

            string _nmTicker = args[0].ToUpperInvariant();

            SendPriceAlert(_nmTicker, _vlSellPrice, _vlBuyPrice);
        }

        private static void SendPriceAlert(string _nmTicker, decimal _vlSell, decimal _vlBuy)
        {
            ClientApi clientApi = new ClientApi();
            
            decimal _vlCurrentPrice;
            string _nmSubject;

            Console.WriteLine($"{DateTime.Now} - Checking {_nmTicker} price recommendation to Sell ({_vlSell}) or Buy ({_vlBuy})...");

            while (true)
            {
                _vlCurrentPrice = clientApi.getPriceFromApi(_nmTicker);

                _nmSubject = _vlCurrentPrice > _vlSell ? "Sell recommended" : _vlCurrentPrice < _vlBuy ? "Buy recommended" : string.Empty;

                if (!string.IsNullOrEmpty(_nmSubject))
                {
                    _nmSubject += $" = Current Price {_vlCurrentPrice}";
                    SendMail.SendMail.SendMailMessagePrices(_nmSubject, string.Empty);
                    Console.WriteLine($"{DateTime.Now} - {_nmSubject}");
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} - No recommendation sent. Current Price: {_vlCurrentPrice}");
                }

                Thread.Sleep(20000);
            }
        }
    }
}
