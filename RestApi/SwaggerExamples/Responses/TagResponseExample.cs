using System;
using RestApi.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestApi.SwaggerExamples.Responses
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "New tag name"
            };
        }
    }
}
