using Microsoft.AspNetCore.Authorization;

namespace individueel_project_1._3_api.Authorization;

public class DoesUserMatchAuthenticationHandler <T>(Func<T, string> userExtractor) : AuthorizationHandler<SameUserRequirement, T>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserRequirement requirement, T resource)
    {
        context.Succeed(requirement);
        
        if (context.User.Identity?.Name == userExtractor(resource))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}