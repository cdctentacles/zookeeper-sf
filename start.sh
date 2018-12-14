#!/usr/bin/env bash

check_errs()
{
  # Function. Parameter 1 is the return code
  if [ "${1}" -ne "0" ]; then
    # make our script exit with the right error code.
    while true; do
      sleep 1;
      echo "error : ${1}";
    done
  fi
}

env

ServiceName="${ServiceName:-fabric:/zookeeper/service}"
NumberOfReplica="${NumberOfReplica:-3}"
MaxNumRetries="${MaxNumRetries:-10}"
WaitInMsPerRetry="${WaitInMsPerRetry:-5000}"
MyIp=`hostname -I`

pushd /setup

# "ServiceName" "NumberOfReplica" "MaxNumRetries" "WaitInMsPerRetry" "MyIP" "DataFolderPath" "ConfFilePath"
LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$FabricCodePath dotnet zookeeper-sf.dll $ServiceName $NumberOfReplica $MaxNumRetries $WaitInMsPerRetry $MyIp /app /app/zoo.cfg
# $ZK_HOME/data $ZK_HOME/conf/zoo.cfg
check_errs $?
popd

/usr/bin/start-zk.sh