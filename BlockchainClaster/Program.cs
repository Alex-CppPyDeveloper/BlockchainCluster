using System;
using BlockChainNode;

using System.Net.Http;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Akka.Dispatch;
using System.Text;
using System.Threading;

namespace BlockchainClaster
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            string url = "chains/main/blocks/head/header";

            List<string> urls = new List<string>() { "https://rpc.tzkt.io/mainnet/", "https://mainnet-tezos.giganode.io/", "https://mainnet.smartpy.io/" };

            string i = null;

            while (Convert.ToInt16(i) != 2 )
            {
                Console.WriteLine("Добавить url?");
                i = Console.ReadLine();

                if (Convert.ToInt16(i) != 2)
                {
                    string new_base_url = Console.ReadLine();
                    urls.Add(new_base_url);
                }
                else break;

            }

            NodeCluster nodeCluster = new NodeCluster(urls);

           
            Head LatestHead = new Head();
            Task task = Task.Factory.StartNew(async() => LatestHead = await nodeCluster.GetLatestHead(url));
            task.Wait();
            // LatestHead = await Task.Run<Head>(() => nodeCluster.GetLatestHead(url));
            Thread.Sleep(5000);
           Task task_return = Task.Factory.StartNew(() =>
           {
               Console.WriteLine($"Head.Url: {LatestHead.Url}");
               Console.WriteLine($"Head.ChainId: {LatestHead.Chain_id}");
               Console.WriteLine($"Head.Hash: {LatestHead.Hash}");
               Console.WriteLine($"Head.Predecessor: {LatestHead.Predecessor}");
               Console.WriteLine($"Head.Level: {LatestHead.Level}");
               Console.WriteLine($"Head.Timestamp: {LatestHead.Timestamp}");
           });
            task_return.Wait();
           // await Task.WhenAll(new[] { task, task_return });
           
        }

    }

}
    

