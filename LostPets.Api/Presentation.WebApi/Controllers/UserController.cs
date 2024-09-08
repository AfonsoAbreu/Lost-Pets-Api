using Application.Exceptions;
using Application.Services.Interfaces;
using AutoMapper;
using Infrastructure.Data.Entities;
using Infrastructure.Facades.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.Controllers.Base;
using Presentation.WebApi.Data.DTOs.ApiErrors;
using Presentation.WebApi.Data.DTOs.Variations;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IJwtFacade _jwtFacade;

        public UserController(UserManager<User> userManager, IUserService userService, IMapper mapper, SignInManager<User> signInManager, IJwtFacade jwtFacade) : base(userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _signInManager = signInManager;
            _jwtFacade = jwtFacade;
        }

        [HttpGet("{id}")]
        public ActionResult<UserProfileDTO> GetById([FromRoute] Guid id)
        {
            bool isCurrentUser = AreUserIdsFromCurrentUser(id);
            User? user = _userService.GetById(id, isCurrentUser);

            if (user == null)
            {
                return NotFound();
            }

            UserProfileDTO userDTO = _mapper.Map<UserProfileDTO>(user);

            return userDTO;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResultDTO>> Login([FromBody] UserLoginDTO login)
        {
            User? user = await _userService.GetByEmailAsync(login.email);

            if (user == null)
            {
                return BadRequest();
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, login.password, false, false);

            if (!signInResult.Succeeded)
            {
                return BadRequest("Invalid e-mail or password.");
            }

            string token = _jwtFacade.GenerateJwt(user);
            UserProfileDTO userDTO = _mapper.Map<UserProfileDTO>(user);
            UserLoginResultDTO userLoginDTO = new()
            {
                token = token,
                user = userDTO,
            };

            return Ok(userLoginDTO);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegister)
        {
            User? existingUser = await _userService.GetByEmailAsync(userRegister.email);

            if (existingUser != null)
            {
                return BadRequest("An user with this e-mail was already registered.");
            }

            User newUser = new()
            {
                Id = Guid.NewGuid(),
                Email = userRegister.email,
                UserName = userRegister.userName,
            };

            if (userRegister.contacts != null)
            {
                ICollection<Contact> contacts = _mapper.Map<ICollection<Contact>>(userRegister.contacts);

                foreach (Contact contact in contacts)
                {
                    contact.UserId = newUser.Id;
                    contact.User = newUser;
                }

                newUser.Contacts = contacts;
            }

            try
            {
                await _userService.CreateUserAsync(newUser, userRegister.password);
            }
            catch (UnsuccessfulOperationDomainException ex)
            {
                return BadRequest(new ApiErrorDTO 
                { 
                    message = ex.Message,
                    content = ex.Data
                });
            }

            return Created();
        }
    }
}
