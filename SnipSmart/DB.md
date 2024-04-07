## make it work
export PATH="$PATH:$HOME/.dotnet/tools/"

## Migrations
dotnet ef migrations add InitialCreate
dotnet ef database update


