using AutoMapper;
using CardStorageService.Data;
using CardStorageService.Models;
using CardStorageService.Models.Requests;

namespace CardStorageService.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Card, CardDto>();
            CreateMap<CreateCardRequest, Card>();

            CreateMap<CreateClientRequest, Client>();
        }
    }
}
