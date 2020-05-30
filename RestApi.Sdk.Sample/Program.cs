using System;
using System.Threading.Tasks;
using Refit;
using RestApi.Contracts.V1.Requests;

namespace RestApi.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
            var restApiApi = RestService.For<IRestApiApi>("https://localhost:5001", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token)
            });


            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdkaccount@test.com",
                Password = "Test123!"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sdkaccount@test.com",
                Password = "Test123!"
            });

            token = loginResponse.Content.Token;

            var allPosts = await restApiApi.GetAllAsync();

            var createdPost = await restApiApi.CreateAsync(new CreatePostRequest
            {
                Name = "This is created by SDK",
                Tags = new[] { "sdk" }
            });

            var retrievedPost = await restApiApi.GetAsync(createdPost.Content.Id);

            var updatedPost = await restApiApi.UpdateAsync(createdPost.Content.Id, new UpdatePostRequest
            {
                Name = "This is updated by SDK"
            });

            var deletedPost = await restApiApi.DeleteAsync(createdPost.Content.Id);
        }
    }
}
