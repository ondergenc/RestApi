using System;
using RestApi.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace RestApi.SwaggerExamples.Requests
{
    public class CreateTagRequestExample : IExamplesProvider<CreateTagRequest>
    {
        public CreateTagRequest GetExamples()
        {
            return new CreateTagRequest
            {
                TagName = "New tag name"
            };
        }
    }
}
