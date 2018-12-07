FROM wurstmeister/zookeeper

ADD start.sh /usr/bin/start.sh

CMD /usr/sbin/sshd && bash /usr/bin/start.sh