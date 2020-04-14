# AspNetCoreIdentityFramework
Asp Net Core Identity Framework (with Blazor Admin Tool), Tests and Examples

The repository is divided in different solution folders:

- **IdentityFramework**: it contains the following projects:
    
    * **Globe.Identity**: for managing user registration and login, jwt token, different database supports.
    * **Globe.Identity.MultiTenant**: for managing multi tenant access (work in progress).
    * **Globe.Identity.Server** and **Globe.Identity.Server.MultiTenant**: examples to show how to implement a server for managing users (registration and login).
- **IdentityAdministrativeDashboard**: it contains a portal to manage users and roles and associate them based on Balzor Client:

    * **Globe.Identity.AdministrativeDashboard.Server**: backend for registration and login.
    * **Globe.Identity.AdministrativeDashboard.Client**: frontend for registration and login.
    * **Globe.Identity.AdministrativeDashboard.Shared**: it contains DTO shared by the two above projects.

- **WPFLocalizer**: it contains a WPF desktop application based on Prism, that uses the same backend used by the portal, to get the token and communicate with a fake server (Globe.TranslationServer):

    * **Globe.Localizer**: the host that runs the application.
    * **Globe.Platform**: it contains basic services, controls, Prism events and other.
    * **Globe.Platform.Assets**: it contains styles and localized keys.

Additional projects are:

- **Globe.BusinessLogic**: it contains the repository definition.
- **Globe.Infrastructure.EFCore**: it contains Entity Framework repository implementations. 
- **Globe.ResourceServer**: example to show authorization accesses
- **Globe.TranslationServer**: server example for WPF client with in memory strings to translate.

## Run Locally

Follow the above steps:

- Open the solution Properties

- Select *Multiple Startup Projects* set to Start:
    
    - Globe.Identity.AdministrativeDashboard.Server
    - Globe.TranslationServer
    - Globe.Localizer

- Check the endpoints of the *Globe.Localizer* project, inside app.config, in order to be able to:

    - login towards Globe.Identity.AdministrativeDashboard.Server

        The current setting for *IIS Express* configuration is

            https://localhost:44317/api/

    - request data from Globe.TranslationServer

        The current setting for *IIS Express* configuration is

            https://localhost:44360/api/

## Run on Docker

In the solution folder there are two *Dockerfile*, for *Globe.Identity.AdministrativeDashboard.Server* and *Globe.TranslationServer* projects, and the *docker-compose.yml*.

To run the system on Docker it's necessary to have Docker installed.

It's important to see the volumes mapping in the *docker-compose.yml*:

          volumes:
           - /https:/https:rw

On my PC I used *Docker ToolBox* with *VirtualBox*. */https/* is the name that maps the physical path on the computer. The mapping is shown in *Settings* of the virtual machine used, in the section *Shared Folders*.
In that path must there be the *.pfx* certificate used by the https of the sites

*Example*

For Globe.TranslationServer project I have:

        services:
         globe-translation-server:
          build:
           context: ./
           dockerfile: globe.translationserver.Dockerfile
          restart: always
          ports:
           - "7000:80"
           - "7005:443"
          environment:
           ASPNETCORE_ENVIRONMENT: Docker
           ASPNETCORE_URLS: https://+;http://+
           ASPNETCORE_HTTPS_PORT: 7005
           ASPNETCORE_Kestrel__Certificates__Default__Path: /https/<<your certificate>>.pfx
           ASPNETCORE_Kestrel__Certificates__Default__Password: <<your password>>
           ASPNETCORE_JwtAuthenticationOptions__SecretKey: <<your secret key>>
          volumes:
           - /https:/https:rw

I mapped */https* of my PC on */https:rw* of the Docker. And */https* on my PC is mapped by VirtualBox on a phisical path.

Then I specified the *ASPNETCORE_Kestrel__Certificates__Default__Path* where there is the *.pfx* certificate. The certificate is a development one generate automatically by .NET during the first running in https mode (a message box appears). And then I exported it with *mmc* command and the *Certificates* snap-in (there are many tutorials for doing that, see References). 

After the above considerations, it's possible to run the System on Docker. Follow the next steps:

- Open a PowerShell console with administrative rights.
- Go unde the solution folder.
- In order to build the images, type:

        docker-compose build

- In order to run the containers, type:

        docker-compose up

- Open the app.config of the *Globe.Client.Localizer* and change the endpoints with the ones for the containers (with the comment *endpoint for docker*)
- Inject the right http client service in order to by pass the certificate. In *LocalizerModule.cs*, change the line:

                    containerRegistry.Register<IAsyncSecureHttpClient, BearerHttpClient>();

    with:

                    containerRegistry.Register<IAsyncSecureHttpClient, ByPassCertificateHttpClient>();
- Inject the right http client service in order to by pass the certificate. In *App.xaml.cs*, change the line:

                    containerRegistry.Register<IAsyncLoginService, BearerHttpClient>();

    with:

                    containerRegistry.Register<IAsyncLoginService, ByPassCertificateHttpLoginService>();


- Build and Run the client.

## References

### Docker

- <https://medium.com/@the.green.man/set-up-https-on-local-with-net-core-and-docker-7a41f030fc76>
- <https://jack-vanlightly.com/blog/2017/9/24/how-to-connect-to-your-local-sql-server-from-inside-docker>

### Certificates

- <https://www.meziantou.net/custom-certificate-validation-in-dotnet.htm>

### Database

- <https://support.esri.com/en/technical-article/000009958>
- <https://social.msdn.microsoft.com/Forums/vstudio/en-US/cbb31b88-8d95-4afe-8ce1-5290aedc1e46/login-failed-for-user-sa-net-sqlclient-data-provider?forum=sqldataaccess>