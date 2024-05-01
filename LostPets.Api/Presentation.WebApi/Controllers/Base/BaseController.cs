using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers.Base
{
    public abstract class BaseController(UserManager<User> userManager) : ControllerBase
    {

        private readonly UserManager<User> _userManager = userManager;

        protected async Task<User?> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        protected async Task<bool> AreUserIdsFromCurrentUser(params Guid?[] ids)
        {
            var user = await GetCurrentUser();

            if (user == null)
            {
                return false;
            }

            return ids.All(id => user.Id == id);
        }

    }
}
