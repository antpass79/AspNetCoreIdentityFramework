using Globe.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Tests.Mocks
{
    public class FakeUserManager : UserManager<GlobeUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<GlobeUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<GlobeUser>>().Object,
                  new IUserValidator<GlobeUser>[0],
                  new IPasswordValidator<GlobeUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<GlobeUser>>>().Object)
        { }

        public static FakeUserManager Mock(List<GlobeUser> users = null)
        {
            users = users == null ? new List<GlobeUser>() : users;

            PasswordHasher<GlobeUser> hasher = new PasswordHasher<GlobeUser>();

            var userManager = new Mock<FakeUserManager>();
            userManager.Object.UserValidators.Add(new UserValidator<GlobeUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<GlobeUser>());

            userManager
                .Setup(x => x.DeleteAsync(It.IsAny<GlobeUser>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager
                .Setup(x => x.CreateAsync(It.IsAny<GlobeUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<GlobeUser, string>((appUser, password) =>
                {
                    appUser.PasswordHash = hasher.HashPassword(appUser, password);
                    users.Add(appUser);
                });
            userManager
                .Setup(x => x.UpdateAsync(It.IsAny<GlobeUser>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns<string>((userName) => Task.FromResult(users.Find(user => user.UserName == userName)));
            userManager
                .Setup(x => x.CheckPasswordAsync(It.IsAny<GlobeUser>(), It.IsAny<string>()))
                .Returns<GlobeUser, string>((user, password) =>
                {
                    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
                    return Task.FromResult(result == PasswordVerificationResult.Success);
                });

            return userManager.Object;
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
