using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Contracts.V1;
using RestApi.Contracts.V1.Requests;
using RestApi.Contracts.V1.Responses;
using RestApi.Domain;
using RestApi.Extensions;
using RestApi.Services;

namespace RestApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
    public class TagController: Controller
    {
        private readonly IPostService _postService;

        public TagController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetAllTagsAync());
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute]string tagName)
        {
            var tag = await _postService.GetTagByNameAsync(tagName);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(await _postService.GetTagByNameAsync(tagName));
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        [ProducesResponseType(typeof(TagResponse), 201)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.TagName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _postService.CreateTagAsync(newTag);
            if (!created)
            {
                return BadRequest(new { error = "Unable to create tag" });
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);

            var response = new TagResponse { Name = newTag.Name };

            return Created(locationUri, response);
        }

        [HttpDelete(ApiRoutes.Tags.Delete)]
        //[Authorize(Roles = "Admin")]
        [Authorize(Policy = "MustWorkForRestApi")]
        public async Task<IActionResult> Delete([FromRoute] string tagName)
        {
            var deleted = await _postService.DeleteTagAsync(tagName);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
