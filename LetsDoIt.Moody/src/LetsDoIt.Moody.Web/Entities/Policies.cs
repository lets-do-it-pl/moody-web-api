using Microsoft.AspNetCore.Authorization;

namespace LetsDoIt.Moody.Web.Entities
{
    using Application.Constants;

    public static class Policies
    {
        public static AuthorizationPolicy AdminPolicy() =>
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserTypeConstants.Admin)
                .Build();

        public static AuthorizationPolicy StandardPolicy() => 
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserTypeConstants.Standard)
                .Build();

        public static AuthorizationPolicy ClientPolicy() =>
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserTypeConstants.Client)
                .Build();
    }
}
