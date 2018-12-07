using System;
using System.Linq;
using System.Threading;

using Microsoft.ServiceFabric.Services.Client;

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

                var cancellationSource = new CancellationTokenSource(waitInMsPerRetry);
                var partitionFindTask = partitionResolver.ResolveAsync(new Uri(serviceName), ServicePartitionKey.Singleton, cancellationSource.Token);

                try {
                    partitionFindTask.Wait();
                } catch (Exception) {}

                if (partitionFindTask.IsCompletedSuccessfully)
                {
                    var endpointAddress = partitionFindTask.Result.Endpoints.Select(x => x.Address);
                    return new ConditionalValue<string[]>(endpointAddress.ToArray());
                }
            }

            return new ConditionalValue<string[]>();
        }
    }
}