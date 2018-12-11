#!/usr/bin/env bash

check_errs()
{
  # Function. Parameter 1 is the return code
  if [ "${1}" -ne "0" ]; then
    # make our script exit with the right error code.
    exit ${1}
  fi
}

ServiceName="${ServiceName:-fabric:/zookeeper/service}"
NumberOfReplica="${NumberOfReplica:-3}"
MaxNumRetries="${MaxNumRetries:-10}"
WaitInMsPerRetry="${WaitInMsPerRetry:-5000}"
MyIp=`hostname -I`

# "ServiceName" "NumberOfReplica" "MaxNumRetries" "WaitInMsPerRetry" "MyIP" "DataFolderPath" "ConfFilePath"
/setup/zookeeper-sf $ServiceName $NumberOfReplica $MaxNumRetries $WaitInMsPerRetry $MyIp $ZK_HOME/data $ZK_HOME/conf/zoo.cfg
check_errs()

/usr/bin/start-zk.sh