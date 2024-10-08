﻿using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Entities;
using Presentation.WebApi.Controllers.Base;
using AutoMapper;
using Application.Exceptions;
using Presentation.WebApi.Data.DTOs.Variations;


namespace Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : BaseController
    {

        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper, UserManager<User> userManager)
            : base(userManager) {
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status201Created)]
        public ActionResult<CommentDTO> Add([FromBody] CommentDTOWithRequiredMissingPetId commentDto)
        {
            Comment comment = _mapper.Map<Comment>(commentDto);

            Guid? userId = GetCurrentUserId();

            if (userId == null)
            {
                return Forbid();
            }

            comment.UserId = userId.Value;

            _commentService.Add(comment);

            CommentDTO createdCommentDTO = _mapper.Map<CommentDTO>(comment);

            return CreatedAtAction(null, null, createdCommentDTO);
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Remove([FromRoute] Guid id)
        {
            Comment? comment = _commentService.GetById(id);

            if (comment == null)
            {
                return NotFound();
            }

            bool isNotOwnedByCurrentUser = !AreUserIdsFromCurrentUser(comment.UserId);
            if (isNotOwnedByCurrentUser)
            {
                return Forbid();
            }

            _commentService.Remove(comment);

            return NoContent();
        }

        [HttpPut("{id}"), Authorize]
        public ActionResult<CommentDTO> Edit([FromRoute] Guid id, [FromBody] CommentDTO commentDto)
        {
            commentDto.id = id;

            Comment receivedComment = _mapper.Map<Comment>(commentDto);

            Guid? userId = GetCurrentUserId();

            if (!userId.HasValue) 
            {
                return Forbid();
            }

            receivedComment.UserId = userId.Value;

            try
            {
                receivedComment = _commentService.Update(receivedComment);
            }
            catch (ResourceNotFoundDomainException ex)
            {
                return NotFound(ex.Message);
            }
            catch (MismatchedUserDomainException ex)
            {
                return Forbid(ex.Message);
            }
            catch (MismatchedRelationDomainException ex)
            {
                return BadRequest(ex.Message);
            }

            CommentDTO resultingCommentDTO = _mapper.Map<CommentDTO>(receivedComment);

            return Ok(resultingCommentDTO);
        }
    }
}
