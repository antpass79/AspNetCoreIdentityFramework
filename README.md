# AspNetCoreIdentityFramework
Asp Net Core Identity Framework (with Blazor Admin Tool), Tests and Examples

The repository is divided in different solution folders:

- **IdentityFramework**: it contains the following projects:
    
    * **Globe.Identity**: for managing user registration and login, jwt token, different database supports.
    * **Globe.Identity.MultiTenant**: for managing multi tenant access.
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