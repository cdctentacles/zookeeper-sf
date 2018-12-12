using System;
using System.Linq;
using System.Threading;

using Microsoft.ServiceFabric.Services.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace zookeeper_sf
{
    static class FindService
    {
        public static ConditionalValue<string[]> FindServiceIps(string serviceName, int numReplica, int maxNumRetries, int waitInMsPerRetry)
        {
            var partitionResolver = ServicePartitionResolver.GetDefault();

            var retryCount = 0;
            while (retryCount < maxNumRetries)
            {
                retryCount += 1;

                Console.WriteLine($"Attempt to find service ips : {retryCount}");

                var cancellationSource = new CancellationTokenSource(waitInMsPerRetry);
                var partitionFindTask = partitionResolver.ResolveAsync(new Uri(serviceName), ServicePartitionKey.Singleton, cancellationSource.Token);

                try {
                    partitionFindTask.Wait();
                } catch (Exception ex)
                {
                    Console.WriteLine($"Exception from FindServiceIps task : {ex}");
                }

                if (partitionFindTask.IsCompletedSuccessfully)
                {
                    var endpoints = partitionFindTask.Result.Endpoints;
                    // {"Endpoints":{"ServiceEndpoint":"10.0.0.13:9767"}},{"Endpoints":{"ServiceEndpoint":"10.0.0.22:9767"}},{"Endpoints":{"ServiceEndpoint":"10.0.0.4:9767"}}
                    var endpointAddress = endpoints
                        .Select(x => x.Address)
                        .Select(address => {  // {"Endpoints":{"ServiceEndpoint":"10.0.0.13:9767"}}
                            var jsonObj = JObject.Parse(address);
                            // validate that we have 2 endpoints : "LeaderEndpoint" and "LeaderElectionEndpoint"
                            var leaderEndpoint = jsonObj["Endpoints"]["LeaderEndpoint"].ToObject<String>();
                            var leaderElectionEndpoint = jsonObj["Endpoints"]["LeaderElectionEndpoint"].ToObject<String>();
                            return leaderEndpoint + ":" + PortOf(leaderElectionEndpoint);
                        })
                        .ToArray();

                    Array.Sort(endpointAddress);
                    var serverAddress = endpointAddress
                        .Select((zooServerEndpoint, index) => "server." + (index + 1) + "=" + zooServerEndpoint)
                        .ToArray();

                    if (endpoints.Count < numReplica)
                    {
                        Console.Error.WriteLine($"Not found {numReplica} : {String.Join(',', serverAddress)}");
                    }
                    else
                    {
                        return new ConditionalValue<string[]>(serverAddress);
                    }
                }
                else
                {
                    Console.WriteLine($"Task did not complete successfully. Sleep for {waitInMsPerRetry} msecs");
                }

                Thread.Sleep(waitInMsPerRetry);
            }

            return new ConditionalValue<string[]>();
        }

        static string PortOf(string address)
        {
            var portIndex = address.LastIndexOf(':');
            if ((-1 == portIndex) || (portIndex == (address.Length - 1)))
            {
                return "";
            }

            var port = address.Substring(portIndex + 1);
            var portInt = 0;
            if (int.TryParse(port, out portInt))
            {
                return port;
            }

            return "";
        }
    }
}