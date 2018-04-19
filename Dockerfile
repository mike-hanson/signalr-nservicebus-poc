FROM microsoft/windowsservercore

####### PORTS ########
#Main rabbitmq port
EXPOSE 5672
#port mapper daemon (epmd)
EXPOSE 4369
#inet_dist_listen
EXPOSE 35197
#rabbitmq management console
EXPOSE 15672

#set the home directory for erlang so rabbit can find it easily
ENV ERLANG_HOME "c:\program files\erl8.2\erts-8.2"
ENV ERLANG_SERVICE_MANAGER_PATH "c:\program files\erl8.2\erts-8.2"

#install chocolatey
RUN @powershell -NoProfile -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"

#install rabbitmq
RUN choco install -y rabbitmq

#set up the path to the config file
ENV RABBITMQ_CONFIG_FILE "C:\rabbitmq"

#copy a config file over
COPY ["rabbitmq.config"," C:/"]

#set the startup command to be rabbit
CMD ["C:/Program Files/RabbitMQ Server/rabbitmq_server-3.6.5/sbin/rabbitmq-server.bat"]