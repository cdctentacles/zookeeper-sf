# Zookeeper Service Fabric App

Deploy using:
```powershell
Remove-ServiceFabricComposeDeployment -DeploymentName zookeeper -Force
New-ServiceFabricComposeDeployment -DeploymentName zookeeper -Compose stack.yaml
```

Todo :
* Make sure that no instances of zookeeper are on same node.