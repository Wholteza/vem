## Dotnet setup

install script

export PATH="$HOME/.dotnet/tools:$PATH"
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"

## EF migrations setup

dotnet tool install --global dotnet-ef
dotnet ef migrations add initialcreate
dotnet ef database update

## Running locally

docker compose up --build
