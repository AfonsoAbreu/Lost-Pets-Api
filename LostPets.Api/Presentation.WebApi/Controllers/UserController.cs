using Application.Services.Interfaces;
using AutoMapper;
using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.Controllers.Base;
using Presentation.WebApi.Data.DTOs.Variations;

namespace Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IUserService userService, IMapper mapper) : base(userManager)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetById([FromRoute] Guid id)
        {
            bool isCurrentUser = await AreUserIdsFromCurrentUser(id);
            User? user = _userService.GetById(id, isCurrentUser);

            if (user == null)
            {
                return NotFound();
            }

            UserProfileDTO userDTO = _mapper.Map<UserProfileDTO>(user);

            return userDTO;
        }
    }
}
