FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY . ./BizCardHolder/
RUN dotnet restore BizCardHolder

COPY . ./BizCardHolder/

WORKDIR /app/BizCardHolder
RUN dotnet build -c Debug -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/BizCardHolder/output .
ENTRYPOINT ["dotnet", "BizCard.API.dll"]
