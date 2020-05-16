using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestApi.Contracts.V1;
using RestApi.Domain;

namespace RestApi.Controllers.V1
{
    public class PostsController : Controller
    {
        private List<Post> _posts;

        public PostsController()
        {
            _posts = new List<Post>();
            for (int i = 0; i < 5; i++)
                _posts.Add(new Post { Id = Guid.NewGuid().ToString() });
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }
    }
}
