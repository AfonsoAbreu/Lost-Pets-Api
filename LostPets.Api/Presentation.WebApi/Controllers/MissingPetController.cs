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
using Presentation.WebApi.Data.DTOs.Variations;


namespace Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissingPetController : BaseController
    {

        private readonly IMissingPetService _missingPetService;
        private readonly IMapper _mapper;

        public MissingPetController(IMissingPetService missingPetService, IMapper mapper, UserManager<User> userManager)
            : base(userManager) {
            _missingPetService = missingPetService;
            _mapper = mapper;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(MissingPetDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<MissingPetDTO>> Add([FromBody] MissingPetDTO missingPetDto)
        {
            User? user = await GetCurrentUser();
            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            missingPetDto.user = userDTO;
            missingPetDto.sightings
                ?.ToList()
                .ForEach(s => s.user = userDTO);

            MissingPet missingPet = _mapper.Map<MissingPet>(missingPetDto);

            try
            {
                _missingPetService.Add(missingPet);
            } 
            catch (ValidationDomainException ex)
            {
                return BadRequest(ex.Message);
            }

            MissingPetDTO createdMissingPetDTO = _mapper.Map<MissingPetDTO>(missingPet);

            return CreatedAtAction(nameof(GetById), new { id = missingPet.Id }, createdMissingPetDTO);
        }

        [HttpGet("{id}")]
        public ActionResult<MissingPetDTO> GetById([FromRoute] Guid id)
        {
            MissingPet? missingPet = _missingPetService.GetById(id);

            if (missingPet == null)
            {
                return NotFound();
            }

            MissingPetDTO missingPetDTO = _mapper.Map<MissingPetDTO>(missingPet);

            return Ok(missingPetDTO);
        }

        [HttpGet]
        public ActionResult<IEnumerable<MissingPetDTO>> Search([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radius, [FromQuery] int page = 1, [FromQuery] int itemsPerPage = 10) 
        { 
            LocationDTO locationDTO = new LocationDTO()
            {
                latitude = latitude,
                longitude = longitude,
            };
            Point location = _mapper.Map<Point>(locationDTO);

            List<MissingPet> missingPets = _missingPetService.SearchBylocationAndRadius(location, radius, page, itemsPerPage)
                .ToList();

            if (missingPets.Count == 0)
            {
                return NoContent();
            }

            IEnumerable<MissingPetDTO> missingPetDTOs = _mapper.Map<IEnumerable<MissingPetDTO>>(missingPets);

            return Ok(missingPetDTOs);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<MissingPetDTO>> Edit([FromRoute] Guid id, [FromBody] MissingPetDTOWithOptionalSightings missingPetDTO)
        {
            User? user = await GetCurrentUser();
            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            missingPetDTO.id = id;
            missingPetDTO.user = userDTO;

            foreach (var sighting in missingPetDTO.sightings ?? [])
            {
                sighting.missingPetId = id;
            }
            foreach (var comment in missingPetDTO.comments ?? [])
            {
                comment.missingPetId = id;
                comment.user ??= userDTO;
            }

            missingPetDTO.sightings = missingPetDTO.sightings
                ?.Where(sighting => sighting.user == userDTO)
                .ToList();
            missingPetDTO.comments = missingPetDTO.comments
                ?.Where(comment => comment.user == userDTO)
                .ToList();

            MissingPet receivedMissingPet = _mapper.Map<MissingPet>(missingPetDTO);

            try
            {
                receivedMissingPet = _missingPetService.Update(receivedMissingPet);
            }
            catch (ResourceNotFoundDomainException ex)
            {
                return NotFound(ex.Message);
            }
            catch (MismatchedUserDomainException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (MismatchedRelationDomainException ex)
            {
                return BadRequest(ex.Message);
            }

            MissingPetDTO resultingMissingPetDTO = _mapper.Map<MissingPetDTO>(receivedMissingPet);

            return Ok(resultingMissingPetDTO);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            MissingPet? missingPet = _missingPetService.GetById(id);

            if (missingPet == null)
            {
                return NotFound();
            }

            User? user = await GetCurrentUser();
            if (user?.Id != missingPet.UserId)
            {
                return Forbid();
            }

            _missingPetService.Remove(missingPet);

            return NoContent();
        }

        [HttpDelete("{id}/deactivate"), Authorize]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id)
        {
            MissingPet? missingPet = _missingPetService.GetById(id);

            if (missingPet == null)
            {
                return NotFound();
            }

            User? user = await GetCurrentUser();
            if (user?.Id != missingPet.UserId)
            {
                return Forbid();
            }

            _missingPetService.Deactivate(missingPet);

            return NoContent();
        }
    }
}
