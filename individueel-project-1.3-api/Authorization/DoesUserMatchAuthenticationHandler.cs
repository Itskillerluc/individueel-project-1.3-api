using Microsoft.AspNetCore.Authorization;

namespace individueel_project_1._3_api.Authorization;

public class DoesUserMatchAuthenticationHandler <T>(Func<T, string, bool> validator) : AuthorizationHandler<SameUserRequirement, T>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserRequirement requirement, T resource)
    {
        context.Succeed(requirement);
        
        if (validator(resource, context.User.Identity?.Name!))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}