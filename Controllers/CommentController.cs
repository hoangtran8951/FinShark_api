using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Dto.Comment;
using apiSTockapi.Extensions;
using apiSTockapi.Helpers;
using apiSTockapi.Interfaces;
using apiSTockapi.Mappers;
using apiSTockapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace apiSTockapi.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IFMPService _fMPService;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, IFMPService fMPService, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _fMPService = fMPService;
            _userManager = userManager;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query)
        {
            var commentModels = await _commentRepo.GetAllAsync(query);
            if(commentModels.Count == 0)
                return StatusCode(500, "Dont have any comments");
            var commentDtos = commentModels.Select(c => c.FromModelToDto());
            return Ok(commentDtos);
            // return Ok(commentModels);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var commentModel = await _commentRepo.GetById(id);
            if(commentModel == null)
                return BadRequest("This comment doesn't exist");
            
            return Ok(commentModel.FromModelToDto());
        }
        [HttpPost("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto createCommentDto, [FromRoute] string symbol)
        {
            var stockModel = await _stockRepo.GetBySymbol(symbol);
            if(stockModel == null) // create new stock by information provided from FMP
            {
                stockModel = await _fMPService.GetStockInfoFromSymbol(symbol);
                if(stockModel == null)
                    return BadRequest("This stock does not exist");
                else 
                    await _stockRepo.CreateAsync(stockModel);
            }
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = createCommentDto.FromCreateDtoToModel(stockModel.Id);
            commentModel.AppUserId = appUser.Id;
            commentModel = await _commentRepo.CreateAsync(commentModel);
            if(commentModel == null)
                return StatusCode(500, "Can not create comment");
            return CreatedAtAction(nameof(GetById), new {id = commentModel.Id},  commentModel.FromModelToDto());
        }
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateCommentDto updateCommentDto, [FromRoute] int id)
        {
            var commentModel = updateCommentDto.FromUpdateDtoToModel(id);
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var currentComment = await _commentRepo.GetById(id);
            if(currentComment.AppUserId != appUser.Id) // check if this comment is owned by the user who tries to edit it
                return BadRequest("This is not your comment");

            commentModel = await _commentRepo.UpdateAsync(commentModel);
            if(commentModel == null)
                return StatusCode(500, "Can not update comment");

            return Ok(commentModel.FromModelToDto());
        }
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
             var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var currentComment = await _commentRepo.GetById(id);
            if(currentComment.AppUserId != appUser.Id) // check if this comment is owned by the user who tries to edit it
                return BadRequest("This is not your comment");
            var checkStatus = await _commentRepo.DeleteById(id);
            if(checkStatus == false)
                return StatusCode(500, "Can not delete comment");

            return NoContent();
        }
    }   
}