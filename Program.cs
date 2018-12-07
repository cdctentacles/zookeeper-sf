using System;

namespace zookeeper_sf
{
    class Program
    {
        static Program()
        {
            SFBinaryLoader.Initialize();
        }

        static int Main(string[] args)
        {
            // args should be NumberOfReplicas ServiceName
            string [] expectedArgs = new string[] {"ServiceName", "NumberOfReplica", "MaxNumRetries", "WaitInMsPerRetry"};
            if (args.Length != expectedArgs.Length)
            {
                Console.Error.WriteLine("Expected args : {0}", String.Join(',', expectedArgs));
                return -1;
            }

            var serviceName = args[0];
            var numReplica = int.Parse(args[1]);
            var maxNumRetries = int.Parse(args[2]);
            var waitInMsPerRetry = int.Parse(args[3]);

            var retVal = FindService.FindServiceIps(serviceName, numReplica, maxNumRetries, waitInMsPerRetry);
            if (retVal.HasValue())
            {
                Console.WriteLine(String.Join(',', retVal.GetValue()));
                return 0;
            }

            return -2;
        }
    }
}
