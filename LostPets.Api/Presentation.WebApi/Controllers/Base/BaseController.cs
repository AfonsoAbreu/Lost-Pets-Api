using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presentation.WebApi.Controllers.Base
{
    public abstract class BaseController(UserManager<User> userManager) : ControllerBase
    {

        private readonly UserManager<User> _userManager = userManager;

        protected Guid? GetCurrentUserId()
        {
            Claim? jtiClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (jtiClaim == null) 
            {
                return null;
            }

            Guid claimValue;

            if (!Guid.TryParse(jtiClaim.Value, out claimValue)) 
            {
                return null;
            }

            return claimValue;
        }

        protected async Task<User?> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        protected async Task<bool> AreUserIdsFromCurrentUser(params Guid?[] ids)
        {
            Guid? userId = GetCurrentUserId();

            if (!userId.HasValue)
            {
                return false;
            }

            return ids.All(id => userId.Value == id);
        }

    }
}
