.NET 5 - migrations are easier


dotnet-ef migrations add init --project SamuraiApp.Data

create sql script for migration:

`dotnet-ef migrations script  --project SamuraiApp.Data`
apply migrations to database:
`dotnet-ef database update  --project SamuraiApp.Data`

### List Migrations
`dotnet-ef migrations list`


```
cd SamuraiApp.Data/  
dotnet-ef migrations add init-author --startup-project ../SamuraiAPI/SamuraiAPI.csproj
dotnet-ef database update --startup-project ../SamuraiAPI/SamuraiAPI.csproj
```

### Remove Migration

-- last good migration
```
dotnet-ef database update 20201112160511_newsprocs --startup-project ../SamuraiAPI/SamuraiAPI.csproj
dotnet-ef migrations remove --startup-project ../SamuraiAPI/SamuraiAPI.csproj
```