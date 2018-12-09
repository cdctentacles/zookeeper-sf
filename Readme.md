Todo :
* Add dotnet to the Dockerfile
* Build code using dockerimage and mount code in the image
* Start the zookeeper if tool returns success.

Next iteration:
* Create a separate service and lets ask from it : list of all ips and myid.
    * We need separate service because during failover, if ip change, we will still be using same id.