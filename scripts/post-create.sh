dotnet dev-certs https
dotnet user-secrets init --project Vem/Vem.csproj
dotnet user-secrets set --project Vem/Vem.csproj "Postgresql:ConnectionString" "Server=db;Port=5432;Database=postgres;User Id=postgres;Password=Password1234!;"