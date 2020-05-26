using System.Linq;
using AutoMapper;
using RestApi.Contracts.V1.Responses;
using RestApi.Domain;

namespace RestApi.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(destination => destination.Tags,
                           option => option.MapFrom(source => source.Tags.Select(s => new TagResponse { Name = s.TagName })));
            CreateMap<Tag, TagResponse>();
        }
    }
}
