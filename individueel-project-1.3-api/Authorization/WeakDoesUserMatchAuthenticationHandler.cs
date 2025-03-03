using Microsoft.AspNetCore.Authorization;

namespace individueel_project_1._3_api.Authorization;

public class WeakDoesUserMatchAuthenticationHandler <T>(Func<T, string, bool> validator) : AuthorizationHandler<SameUserRequirement, T>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserRequirement requirement, T resource)
    {
        if (validator(resource, context.User.Identity?.Name!))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}