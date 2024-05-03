using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Entities;
using Presentation.WebApi.Controllers.Base;
using AutoMapper;
using NetTopologySuite.Geometries;
using Application.Exceptions;


namespace Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SightingController : BaseController
    {

        private readonly ISightingService _sightingService;
        private readonly IMapper _mapper;

        public SightingController(ISightingService sightingService, IMapper mapper, UserManager<User> userManager)
            : base(userManager) {
            _sightingService = sightingService;
            _mapper = mapper;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SightingDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<SightingDTO>> Add([FromBody] SightingDTOWithRequiredMissingPetId sightingDto)
        {
            User? currentUser = await GetCurrentUser();

            if (currentUser == null)
            {
                return Unauthorized();
            }

            sightingDto.userId = currentUser.Id;

            Sighting sighting = _mapper.Map<Sighting>(sightingDto);

            _sightingService.Add(sighting);

            SightingDTO createdSightingDTO = _mapper.Map<SightingDTO>(sighting);

            return CreatedAtAction(null, null, createdSightingDTO);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            Sighting? sighting = _sightingService.GetById(id);

            if (sighting == null)
            {
                return NotFound();
            }

            User? user = await GetCurrentUser();
            if (user?.Id != sighting.UserId)
            {
                return Forbid();
            }

            _sightingService.Remove(sighting);

            return NoContent();
        }
    }
}
