using System;
using System.IO;

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
            string [] expectedArgs = new string[] {
                "ServiceName",
                "NumberOfReplica",
                "MaxNumRetries",
                "WaitInMsPerRetry",
                "MyIP",
                "DataFolderPath",
                "ConfFilePath"
            };

            if (args.Length != expectedArgs.Length)
            {
                Console.Error.WriteLine("Expected args : {0}", String.Join(',', expectedArgs));
                return 1;
            }

            Console.WriteLine($"Arguments: {String.Join(' ', expectedArgs)}");

            var serviceName = args[0];
            var numReplica = int.Parse(args[1]);
            var maxNumRetries = int.Parse(args[2]);
            var waitInMsPerRetry = int.Parse(args[3]);
            var myIp = args[4];
            var dataFolderPath = args[5];
            var confFilePath = args[6];

            var retVal = FindService.FindServiceIps(serviceName, numReplica, maxNumRetries, waitInMsPerRetry);
            if (retVal.HasValue())
            {
                var endpoints = retVal.GetValue();
                // write endpoints to confFile
                Console.WriteLine("Endpoints : {0}", String.Join(',', endpoints));
                using (var writer = File.AppendText(confFilePath))
                {
                    foreach (var endpoint in endpoints)
                    {
                        writer.WriteLine(endpoint);
                    }
                    writer.Flush();
                }

                // also adds a file `myid` to "data" folder
                var myId = 1;
                foreach (var endpoint in endpoints)
                {
                    if (endpoint.Contains(myIp))
                    {
                        break;
                    }
                    myId += 1;
                }

                File.WriteAllText(Path.Combine(dataFolderPath, "myid"), myId.ToString());
                return 0;
            }

            return 2;
        }
    }
}
