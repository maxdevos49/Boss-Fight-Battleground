#!/bin/bash

### Init
cd /var/Bfb/;
rm -rf /var/Bfb/ks_5
git clone -b develop https://git.linux.iastate.edu/cs309/fall2019/ks_5.git


### Deploy Web Server

cd /var/Bfb/ks_5/BFB/BFB.Web;

dotnet publish --configuration Release;

rm -rf /var/www/BfbWeb
mv ./bin/Release/netcoreapp2.2/publish/ /var/www/BfbWeb;

### Deploy Game Server

cd /var/Bfb/ks_5/BFB/BFB.Server;

dotnet publish --configuration Release;

rm -rf /var/www/BfbServer
mv ./bin/Release/netcoreapp2.2/publish/ /var/www/BfbServer;