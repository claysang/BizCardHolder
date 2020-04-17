FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build
WORKDIR /app

COPY . ./BizCardHolder/
RUN dotnet restore BizCardHolder

WORKDIR /app/BizCardHolder
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS runtime
WORKDIR /app
COPY --from=build /app/BizCardHolder/output .
ENTRYPOINT ["dotnet", "BizCard.API.dll"]
