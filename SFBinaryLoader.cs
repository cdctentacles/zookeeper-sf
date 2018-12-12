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

/*
Environment variables:
Fabric_Endpoint_LeaderElectionEndpoint=3888
LS_COLORS=rs=0:di=01;34:ln=01;36:mh=00:pi=40;33:so=01;35:do=01;35:bd=40;33;01:cd=40;33;01:or=40;31;01:su=37;41:sg=30;43:ca=30;41:tw=30;42:ow=34;42:st=37;44:ex=01;32:*.tar=01;31:*.tgz=01;31:*.arj=01;31:*.taz=01;31:*.lzh=01;31:*.lzma=01;31:*.tlz=01;31:*.txz=01;31:*.zip=01;31:*.z=01;31:*.Z=01;31:*.dz=01;31:*.gz=01;31:*.lz=01;31:*.xz=01;31:*.bz2=01;31:*.bz=01;31:*.tbz=01;31:*.tbz2=01;31:*.tz=01;31:*.deb=01;31:*.rpm=01;31:*.jar=01;31:*.war=01;31:*.ear=01;31:*.sar=01;31:*.rar=01;31:*.ace=01;31:*.zoo=01;31:*.cpio=01;31:*.7z=01;31:*.rz=01;31:*.jpg=01;35:*.jpeg=01;35:*.gif=01;35:*.bmp=01;35:*.pbm=01;35:*.pgm=01;35:*.ppm=01;35:*.tga=01;35:*.xbm=01;35:*.xpm=01;35:*.tif=01;35:*.tiff=01;35:*.png=01;35:*.svg=01;35:*.svgz=01;35:*.mng=01;35:*.pcx=01;35:*.mov=01;35:*.mpg=01;35:*.mpeg=01;35:*.m2v=01;35:*.mkv=01;35:*.webm=01;35:*.ogm=01;35:*.mp4=01;35:*.m4v=01;35:*.mp4v=01;35:*.vob=01;35:*.qt=01;35:*.nuv=01;35:*.wmv=01;35:*.asf=01;35:*.rm=01;35:*.rmvb=01;35:*.flc=01;35:*.avi=01;35:*.fli=01;35:*.flv=01;35:*.gl=01;35:*.dl=01;35:*.xcf=01;35:*.xwd=01;35:*.yuv=01;35:*.cgm=01;35:*.emf=01;35:*.axv=01;35:*.anx=01;35:*.ogv=01;35:*.ogx=01;35:*.aac=00;36:*.au=00;36:*.flac=00;36:*.mid=00;36:*.midi=00;36:*.mka=00;36:*.mp3=00;36:*.mpc=00;36:*.ogg=00;36:*.ra=00;36:*.wav=00;36:*.axa=00;36:*.oga=00;36:*.spx=00;36:*.xspf=00;36:
Fabric_Folder_App_Temp=/mnt/sfroot/_App/zookeeper_App1/temp
Fabric_Folder_App_Log=/mnt/sfroot/_App/zookeeper_App1/log
FabricLogRoot=/mnt/sfroot/log/Containers/sf-1-05c5ce5c-4116-c948-b7d7-c82b2ce61832_7985139d-1c72-ce43-babf-b64d31a619ba
Fabric_NetworkingMode=Other
Fabric_Endpoint_IPOrFQDN_ClientEndpoint=10.0.0.4
Fabric_Endpoint_IPOrFQDN_LeaderElectionEndpoint=10.0.0.4
PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin
Fabric_ContainerName=sf-1-05c5ce5c-4116-c948-b7d7-c82b2ce61832_7985139d-1c72-ce43-babf-b64d31a619ba
Fabric_ApplicationId=zookeeper_App1
PWD=/setup
UserLogsDirectory=/mnt/logs/zookeeper_docker/5e7e7189-10e2-5f4e-bd71-11e765759898/7985139d-1c72-ce43-babf-b64d31a619ba
JAVA_HOME=/usr/lib/jvm/java-7-openjdk-amd64
Fabric_IsCodePackageActivatorHost=false
Fabric_Folder_Application_OnHost=/mnt/sfroot/_App/zookeeper_App1
Fabric_ServicePackageName=zookeeperpkg
Fabric_Endpoint_IPOrFQDN_LeaderEndpoint=10.0.0.4
SHLVL=1
HOME=/root
Fabric_Endpoint_LeaderEndpoint=2888
Fabric_ServicePackageVersionInstance=1.0:1.0:131890629214323760
Fabric_RuntimeConnectionAddress=
Fabric_NET-0-[Other]=Other
Fabric_IsContainerHost=true
Fabric_ApplicationHostId=05c5ce5c-4116-c948-b7d7-c82b2ce61832
Fabric_ServiceName=fabric:/zookeeper/service
LESSOPEN=| /usr/bin/lesspipe %s
ZOOKEEPER_VERSION=3.4.9
FabricDataRoot=/mnt/sfroot/log
LESSCLOSE=/usr/bin/lesspipe %s %s
FabricCodePath=/opt/microsoft/servicefabric/bin/Fabric/Fabric.Code
Fabric_Folder_Application=/mnt/sfroot/_App/zookeeper_App1
Fabric_CodePackageInstanceSeqNum=131890231884137207
Fabric_ServicePackageActivationId=7985139d-1c72-ce43-babf-b64d31a619ba
_=/usr/bin/env
*/