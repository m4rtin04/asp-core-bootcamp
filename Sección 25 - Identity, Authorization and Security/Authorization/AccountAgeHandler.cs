using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Sección_25___Identity__Authorization_and_Security.Models;

namespace Sección_25___Identity__Authorization_and_Security.Authorization
{
    public class AccountAgeHandler : AuthorizationHandler<AccountAgeRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountAgeHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                             AccountAgeRequirement requirement)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(context.User);
            if (user == null)
                return;

            double days = (DateTime.UtcNow - user.CreatedAt).TotalDays;

            if (days >= requirement.MinimumDays)
            {
                context.Succeed(requirement);
            }
        }
    }
}
