# build and copy
dotnet publish
xcopy.exe /EIYS C:\Users\asnegi\Documents\gitrepos\cdctentacles\zookeeper-sf\bin\x64\Debug\netcoreapp2.0\publish\. C:\Users\asnegi\Downloads\zk-sf-app\ServiceFabricVolumeDriverPkg\Code\

# remove old packages
Remove-ServiceFabricApplication fabric:/zkapp -Force
Unregister-ServiceFabricApplicationType zookeeper 0.0.1 -Force

$zkapp = "C:\Users\asnegi\Downloads\zk-sf-app"

# install new app again.
Copy-ServiceFabricApplicationPackage -ApplicationPackagePath $zkapp -ApplicationPackagePathInImageStore zkapp -ShowProgress
Register-ServiceFabricApplicationType -ApplicationPathInImageStore zkapp
New-ServiceFabricApplication -ApplicationName fabric:/zkapp -ApplicationTypeName zookeeper -ApplicationTypeVersion 0.0.1

# $datavalidatorpkg='\\asnegi-pc\e$\work\WindowsFabric\out\debug-amd64\bin\winfabrictest\apppackages\coreclr\ubuntu.16.04-x64\BlockStoreDataValidatorPkg\'
# Remove-ServiceFabricApplication fabric:/datavalidatorpackage -Force
# Unregister-ServiceFabricApplicationType BlockStoreDataAppStatelessServiceType 1.0.0 -Force
# Copy-ServiceFabricApplicationPackage -ApplicationPackagePath $datavalidatorpkg -ApplicationPackagePathInImageStore datavalidatorpkg -ShowProgress ; Register-ServiceFabricApplicationType -ApplicationPathInImageStore datavalidatorpkg ; New-ServiceFabricApplication -ApplicationName fabric:/datavalidatorpackage -ApplicationTypeName BlockStoreDataAppStatelessServiceType -ApplicationTypeVersion 1.0.0
