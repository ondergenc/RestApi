using System;
using System.Collections.Generic;
using RestApi.Domain;

namespace RestApi.Services
{
    public interface IPostService
    {
        List<Post> GetPosts();
        Post GetPostById(Guid postId);
    }
}
