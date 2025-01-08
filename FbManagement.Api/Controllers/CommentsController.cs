using FbManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FbManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }
        [HttpGet("{pageId}/{postId}/comments")]
        public async Task<IActionResult> GetCommentsByPostIdAsync(string pageId , string postId)
        {
            try
            {
                var comments = await _commentsService.GetCommentsByPostIdAsync(pageId,postId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching comments.", details = ex.Message });
            }
        }
    }
}
