#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#.Net基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#设置时区
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
#设置容器工作目录
WORKDIR /app
#默认启动端口号
ENV ASPNETCORE_URLS=http://+:30005
EXPOSE 30005/tcp
# 运行 CMD命令
ENTRYPOINT ["dotnet", "Saas.Core.WebApi.dll"]