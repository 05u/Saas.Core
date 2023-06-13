# Saas.Core

基于WebAPI模板搭建的现代化后端框架.

## 制作docker镜像

mkdir /root/dotnet && cd /root/dotnet

git clone

cd /root/dotnet/Saas.Core/Saas.Core.WebApi

docker build -f Dockerfile -t dotnet_webapi .

## 拉取最新代码并运行启动脚本

linux:

cd /root/dotnet/Saas.Core && bash publish.sh

windows:

start publish.bat
