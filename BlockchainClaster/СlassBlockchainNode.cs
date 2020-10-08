using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using System.Net.Http;
using System.Net;
using System.IO;

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace BlockChainNode
{
    class  Head
    {
        public string Url { get; set; }
        public string Chain_id { get; set; }
        public string Hash { get; set; }
        public string Predecessor { get; set; }
        public int Level { get; set; }
        public DateTime Timestamp { get; set; }

        public Head ReturnLatestHead(List<Head> heads)
        {
            var latest_node = new Head();
            var check_Timestamp = new DateTime(2017, 12, 12, 12, 12, 12);
            
            foreach (Head item in heads)
                {

                    if (item.Timestamp > check_Timestamp)
                    {

                        latest_node = item;
                        check_Timestamp = latest_node.Timestamp;

                    }

                }
            return latest_node;
        }

    }
    class СlassBlockchainNode
    {
        public string base_url;

      public СlassBlockchainNode(string base_url)
        {

            this.base_url = base_url ;

        }

        public async Task<Head> GetHead(string url)

        {

            try
            {

                Console.WriteLine("Hello World!");
                HttpClient client = new HttpClient();

                string s = await client.GetStringAsync(url);

                Head parsed = JsonConvert.DeserializeObject<Head>(s);

                /* Console.WriteLine($"\n {url}");
                 Console.WriteLine($"Head.ChainId: {parsed.Chain_id}");
                 Console.WriteLine($"Head.Hash: {parsed.Hash}");
                 Console.WriteLine($"Head.Predecessor: {parsed.Predecessor}");
                 Console.WriteLine($"Head.Level: {parsed.Level}");
                 Console.WriteLine($"Head.Timestamp: {parsed.Timestamp}");*/
                 parsed.Url = url;
                Console.WriteLine("End");
                return parsed;
               
            }

            catch (HttpRequestException e)
            {
                Head parsed = null;
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return parsed;
            }

            
        }
      
    }

  
    class NodeCluster
    {
        public List<string> urls;

        public NodeCluster(List<string> urls)
        {

            this.urls = urls;

        }


        public async Task<Head> GetLatestHead(string url)
        {

            var heads = new List<Head>();

            Console.WriteLine("Start1");


            Parallel.ForEach(urls, item =>
             {
                 СlassBlockchainNode classBlockchain = new СlassBlockchainNode(item);

                    //_ = classBlockchain.GetHead(item + url);
                    Task taskBLockchain = Task.Run(async () => heads.Add(await classBlockchain.GetHead(item + url)));
                 taskBLockchain.Wait();
             });

            Console.WriteLine("Hello");

            var latest_node = new Head();

            latest_node = await Task.Run(() => latest_node.ReturnLatestHead(heads));

            //Task.WaitAll();

            return latest_node;
        }
    }
}
