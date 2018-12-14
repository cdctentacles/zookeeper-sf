# Connect-ServiceFabicCluster before starting the script.
# build and push dockerimage
# change <tag> in this script and ServiceManifest.xml.
docker build . -f Dockerfile -t zookeeper-sf:0.0.19
docker tag zookeeper-sf:0.0.19 ashishnegi1/zookeeper-sf:0.0.19
docker push ashishnegi1/zookeeper-sf:0.0.19

# remove old packages
Remove-ServiceFabricApplication fabric:/zookeeper -Force
Unregister-ServiceFabricApplicationType zookeeper 0.0.18 -Force

$zkapp = ".\AppPackage"

# install new app again.
Copy-ServiceFabricApplicationPackage -ApplicationPackagePath $zkapp -ApplicationPackagePathInImageStore zkapp -ShowProgress
Register-ServiceFabricApplicationType -ApplicationPathInImageStore zkapp
New-ServiceFabricApplication -ApplicationName fabric:/zookeeper -ApplicationTypeName zookeeper -ApplicationTypeVersion 0.0.19
