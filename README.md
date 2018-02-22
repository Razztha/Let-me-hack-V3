# .net_hackv1
Alumina DotnetCore 1.o API Documentation 
[<img src="https://kingtop.visualstudio.com/_apis/public/build/definitions/6eb8c133-9a79-48e2-862f-7346f17a2306/7/badge"/>](https://kingtop.visualstudio.com/nethack2/_build/index?definitionId=7)

### Prerequisites

[Download DotnetCore 1.0 SDK and DotnetCore 1.0 Runtime](https://github.com/dotnet/core/blob/master/release-notes/download-archives/1.0-preview2-download.md)-install DotnetCore 1.0 SDK and DotnetCore 1.0 Runtime
[Download NodeJs](https://nodejs.org/dist/v8.9.1/node-v8.9.1-x64.msi)-NodeJS
[Download VSCode](https://code.visualstudio.com/Download) -VS Code Or Visual Studio 2015 or higher
[Download SQl Server](https://www.microsoft.com/en-us/evalcenter/evaluate-sql-server-2017-rtm) -Sql Server


### Windows Installing
 Create Database in Sql Server 
```
  Database Name : Alumina
```

 Change appsettings.json file According to Your Local Sql Server Configuration(Change Local Sql Server Name)
```
 "ConnectionStrings": {
    "DefaultConnection": "Data Source=Local Sql Server Name;Initial Catalog=Alumina;Integrated Security=True"
  }
```
Restore All Packages 
```
dotnet restore 
```
Update Database using VSCode Terminal
```
Update-database -context AluminaDbContext
```
Run Appliction 
```
dotnet run
```
Go
```
http://localhost:60909/api/values
```
