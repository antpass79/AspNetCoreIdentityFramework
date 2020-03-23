using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Globe.Identity.Shared.Extensions
{
    public static class GlobeFluentValidationExtensions
    {
        public static void AddGlobeFluentValidation(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
        }
    }
}
