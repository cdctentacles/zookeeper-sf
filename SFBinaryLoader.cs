using System;
using System.IO;
using System.Reflection;

namespace zookeeper_sf
{
    static class SFBinaryLoader
    {
        private const string FabricCodePathEnvironmentVariableName = "FabricCodePath";
        private static string SFCodePath;

        static SFBinaryLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadFromFabricCodePath;
        }

        public static void Initialize()
        {
            SFCodePath = Environment.GetEnvironmentVariable(FabricCodePathEnvironmentVariableName, EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(SFCodePath))
            {
                throw new InvalidOperationException("Environment Variable: The path from where to resolve the Service Fabric binaries has not been set.");
            }
        }

        private static Assembly LoadFromFabricCodePath(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;

            try
            {
                string assemblyPath = Path.Combine(SFCodePath, "NS", assemblyName + ".dll");
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }
            }
            catch (Exception e)
            {
                // Supress any Exception so that we can continue to
                // load the assembly through other means
                Console.WriteLine("Exception in LoadFromFabricCodePath={0}", e.ToString());
            }

            return null;
        }
    }
}
