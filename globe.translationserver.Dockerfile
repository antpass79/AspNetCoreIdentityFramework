FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY Globe.TranslationServer/Globe.TranslationServer.csproj /build/Globe.TranslationServer/
RUN dotnet restore ./build/Globe.TranslationServer/Globe.TranslationServer.csproj

COPY ./Globe.TranslationServer ./build/Globe.TranslationServer/
COPY ./Globe.Identity ./build/Globe.Identity/
COPY ./Globe.Infrastructure.EFCore ./build/Globe.Infrastructure.EFCore/
COPY ./Globe.BusinessLogic ./build/Globe.BusinessLogic/

WORKDIR /build/Globe.TranslationServer
RUN dotnet publish ./Globe.TranslationServer.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

COPY --from=build /build/Globe.TranslationServer/out/ .

ENTRYPOINT ["dotnet", "Globe.TranslationServer.dll"]