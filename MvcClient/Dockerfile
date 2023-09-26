#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
 
WORKDIR /app
EXPOSE 80
EXPOSE 443


RUN apt-get update
RUN apt-get update && \
    apt-get install  -y \
        apt-transport-https \
        arping \
        arptables \
        bridge-utils \
        ca-certificates \
        conntrack \
        curl \
        dnsutils \
        ethtool \
        gdb \
        iperf \
        iperf3 \
        iproute2 \
        tar \
        ipset \
        iptables \
        iputils-ping \
        jq \
        kmod \
        ldap-utils \
        less \
        libpcap-dev \
        net-tools \
        netcat \
        netcat-openbsd \
        openssl \
        nfs-kernel-server \
        psmisc \
       # python-dbg \
       # python3-dbg \
        socat \
        software-properties-common \
        strace \
        tcpdump \
        telnet \
        nano \
        tmux \
        tree \
        vim \
        wget && \
    rm -rf /var/lib/apt/lists/*


COPY ["MvcClient.pfx", "app"]

COPY ["IdentityServerrootCA.crt", "/usr/local/share/ca-certificates/IdentityServerrootCA.crt"]
RUN chmod 644 /usr/local/share/ca-certificates/IdentityServerrootCA.crt && update-ca-certificates

COPY ["ApirootCA.crt", "/usr/local/share/ca-certificates/ApirootCA.crt"]
RUN chmod 644 /usr/local/share/ca-certificates/ApirootCA.crt && update-ca-certificates


#COPY ./MvcClientrootCA.crt /usr/local/share/ca-certificates/MvcClientrootCA.crt
#RUN chmod 644 /usr/local/share/ca-certificates/MvcClientrootCA.crt && update-ca-certificates

 
 
 

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["MvcClient/MvcClient.csproj", "MvcClient/"]
RUN dotnet restore "MvcClient/MvcClient.csproj"
COPY . .
WORKDIR "/src/MvcClient"
RUN dotnet build "MvcClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MvcClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MvcClient.dll"]