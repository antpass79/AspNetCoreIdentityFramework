using Globe.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;

namespace Globe.Tests.Mocks
{
    public class FakeSignInManager : SignInManager<GlobeUser>
    {
        public FakeSignInManager(FakeUserManager fakeUserManager)
            : base(fakeUserManager,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<GlobeUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<GlobeUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object,
                  new Mock<IUserConfirmation<GlobeUser>>().Object                  
                  )
        {
        }

        public static FakeSignInManager Mock(FakeUserManager fakeUserManager)
        {
            var signInManager = new Mock<FakeSignInManager>(fakeUserManager);

            signInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<GlobeUser>(), It.IsAny<string>(), false, false))
                .Returns<GlobeUser, string, bool, bool>(async (user, password, isPersistent, lockoutOnFailure) =>
                {
                    if (await fakeUserManager.CheckPasswordAsync(user, password))
                        return SignInResult.Success;

                    return SignInResult.Failed;
                });
            signInManager
                .Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
                .Returns<ClaimsPrincipal>((claimsPrincipal) =>
                {
                    return claimsPrincipal.Identity.IsAuthenticated;
                });

            return signInManager.Object;
        }

        async public static void FillWithMockUsers(UserManager<GlobeUser> userManager)
        {
            var users = new List<GlobeUser>
            {
                new GlobeUser
                {
                    UserName = "user1",
                    Email = "user1@email.com"
                },
                new GlobeUser
                {
                    UserName = "user2",
                    Email = "user2@email.com"
                }
            };

            foreach (var item in users)
            {
                await userManager.CreateAsync(item, $"{item.UserName}@password");
            }
        }
    }
}
