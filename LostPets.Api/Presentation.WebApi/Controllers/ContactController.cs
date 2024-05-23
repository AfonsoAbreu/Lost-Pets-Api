using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Entities;
using Presentation.WebApi.Controllers.Base;
using AutoMapper;
using Application.Exceptions;


namespace Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : BaseController
    {

        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IMapper mapper, UserManager<User> userManager)
            : base(userManager) {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(ContactDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<ContactDTO>> Add([FromBody] ContactDTO contactDto)
        {
            Contact contact = _mapper.Map<Contact>(contactDto);

            Guid? userId = GetCurrentUserId();

            if (!userId.HasValue)
            {
                return Forbid();
            }

            contact.UserId = userId.Value;

            _contactService.Add(contact);

            ContactDTO createdContactDTO = _mapper.Map<ContactDTO>(contact);

            return CreatedAtAction(null, null, createdContactDTO);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            Contact? contact = _contactService.GetById(id);

            if (contact == null)
            {
                return NotFound();
            }

            bool isNotOwnedByCurrentUser = !(await AreUserIdsFromCurrentUser(contact.UserId));
            if (isNotOwnedByCurrentUser)
            {
                return Forbid();
            }

            _contactService.Remove(contact);

            return NoContent();
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<ContactDTO>> Edit([FromRoute] Guid id, [FromBody] ContactDTO contactDto)
        {
            contactDto.id = id;

            Contact receivedContact = _mapper.Map<Contact>(contactDto);

            Guid? userId = GetCurrentUserId();

            if (!userId.HasValue)
            {
                return Forbid();
            }

            receivedContact.UserId = userId.Value;

            try
            {
                receivedContact = _contactService.Update(receivedContact);
            }
            catch (ResourceNotFoundDomainException ex)
            {
                return NotFound(ex.Message);
            }
            catch (MismatchedUserDomainException ex)
            {
                return Forbid(ex.Message);
            }

            ContactDTO resultingContactDTO = _mapper.Map<ContactDTO>(receivedContact);

            return Ok(resultingContactDTO);
        }
    }
}
