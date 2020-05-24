using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using RestApi.Domain;

namespace RestApi.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext
                .Posts
                .Include(s => s.Tags)
                .SingleOrDefaultAsync(s => s.Id == postId);
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var queryable = _dataContext.Posts.AsQueryable();

            return await queryable.Include(s => s.Tags).ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            postToUpdate.Tags?.ForEach(s => s.TagName = s.TagName.ToLowerInvariant());

            await AddNewTags(postToUpdate);
            _dataContext.Posts.Update(postToUpdate);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            post.Tags?.ForEach(s => s.TagName = s.TagName.ToLowerInvariant());

            await AddNewTags(post);
            await _dataContext.Posts.AddAsync(post);

            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(s => s.Id == postId);

            if (post == null)
            {
                return false;
            }

            if (post.UserId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Tag>> GetAllTagsAync()
        {
            return await _dataContext.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            tag.Name = tag.Name.ToLowerInvariant();
            var existingTag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(s => s == tag);

            if (existingTag != null)
                return true;

            await _dataContext.Tags.AddAsync(tag);
            var createdTag = await _dataContext.SaveChangesAsync();
            return createdTag > 0;
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(s => s.Name == tagName.ToLowerInvariant());
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var tag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(s => s.Name == tagName.ToLowerInvariant());

            if (tag == null)
                return true;

            var postTags = await _dataContext.PostTags.Where(s => s.TagName == tagName.ToLowerInvariant()).ToListAsync();

            _dataContext.PostTags.RemoveRange(postTags);
            _dataContext.Tags.Remove(tag);

            return await _dataContext.SaveChangesAsync() > postTags.Count;
        }

        public async Task AddNewTags(Post post)
        {
            foreach (var tag in post.Tags)
            {
                var existingTag = await _dataContext.Tags.SingleOrDefaultAsync(s => s.Name == tag.TagName);
                if (existingTag != null)
                    continue;
                await _dataContext.Tags.AddAsync(new Tag
                {
                    Name = tag.TagName,
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = post.UserId
                });
            }
        }
    }
}
