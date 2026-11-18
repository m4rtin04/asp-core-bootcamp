using Microsoft.AspNetCore.Authorization;

namespace Sección_25___Identity__Authorization_and_Security.Authorization
{
    public class AccountAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumDays { get; }

        public AccountAgeRequirement(int minimumDays)
        {
            MinimumDays = minimumDays;
        }
    }
}
