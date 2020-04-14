FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY ./Globe.Identity.AdministrativeDashboard/Server/Globe.Identity.AdministrativeDashboard.Server.csproj /build/Globe.Identity.AdministrativeDashboard/Server/
RUN dotnet restore ./build/Globe.Identity.AdministrativeDashboard/Server/Globe.Identity.AdministrativeDashboard.Server.csproj

COPY ./Globe.Identity.AdministrativeDashboard/Server ./build/Globe.Identity.AdministrativeDashboard/Server/
COPY ./Globe.Identity.AdministrativeDashboard/Client ./build/Globe.Identity.AdministrativeDashboard/Client/
COPY ./Globe.Identity.AdministrativeDashboard/Shared ./build/Globe.Identity.AdministrativeDashboard/Shared/
COPY ./Globe.Identity ./build/Globe.Identity/
COPY ./Globe.Infrastructure.EFCore ./build/Globe.Infrastructure.EFCore/
COPY ./Globe.BusinessLogic ./build/Globe.BusinessLogic/

WORKDIR /build/Globe.Identity.AdministrativeDashboard/Server
RUN dotnet publish ./Globe.Identity.AdministrativeDashboard.Server.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

COPY --from=build /build/Globe.Identity.AdministrativeDashboard/Server/out/ .

ENTRYPOINT ["dotnet", "Globe.Identity.AdministrativeDashboard.Server.dll"]