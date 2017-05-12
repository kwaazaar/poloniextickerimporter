using System;
using System.Threading;
using WampSharp.V2;

namespace PoloniexTickerImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var channelFactory = new DefaultWampChannelFactory();
            var channel = channelFactory.CreateJsonChannel("wss://api.poloniex.com", "realm1");
            channel.Open().GetAwaiter().GetResult();
            var tickerSubject = channel.RealmProxy.Services.GetSubject("ticker");

            var cancellationTokenSource = new CancellationTokenSource();

            using (var subscr = tickerSubject.Subscribe(evt =>
            {
                var currencyPair = evt.Arguments[0].Deserialize<string>();
                var last = evt.Arguments[1].Deserialize<decimal>();
                Console.WriteLine($"Currencypair: {currencyPair}, Last: {last}");
            }))
            {
                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
                cancellationTokenSource.Cancel();
            }
        }
    }
}