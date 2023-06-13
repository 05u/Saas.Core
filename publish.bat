schtasks /end /tn "Æô¶¯WebAPI"
taskkill /f /im Saas.Core.WebApi.exe


git pull
dotnet publish -c Release

cd Saas.Core.WebApi\bin\Release\net7.0\publish
start Saas.Core.WebApi.exe
