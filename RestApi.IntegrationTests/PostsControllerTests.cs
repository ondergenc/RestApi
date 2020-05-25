using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using RestApi.Contracts.V1;
using RestApi.Domain;
using Xunit;
using RestApi.Contracts.V1.Requests;

namespace RestApi.IntegrationTests
{
    public class PostsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyPost_ReturnsEmptyResponse()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnPost_WhenPostExistsInTheDatabase()
        {
            //Arrange
            await AuthenticateAsync();
            var createdPost = await CreatePostAsync(new CreatePostRequest { Name = "Test post", Tags = new string[] { "Test Tag" } });

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", createdPost.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = (await response.Content.ReadAsAsync<Post>());
            returnedPost.Id.Should().Be(createdPost.Id);
            returnedPost.Name.Should().Be("Test post");
        }
    }
}