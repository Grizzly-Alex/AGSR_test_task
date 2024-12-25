using Hospital_API.Data.Models;
using Hospital_API.Requests;
using Hospital_API.Responses;
using AutoMapper;

namespace Hospital_API.Utilities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Name, PatienNameResponse>().ReverseMap();
        CreateMap<Patient, PatientResponse>().ReverseMap();

        CreateMap<PatientCreateRequest, Patient>().ReverseMap();
        CreateMap<NameCreateRequest, Name>().ReverseMap();

        CreateMap<NameUpdateRequest, Name>().ReverseMap();
        CreateMap<PatientUpdateRequest, Patient>().ReverseMap();
    }
}
