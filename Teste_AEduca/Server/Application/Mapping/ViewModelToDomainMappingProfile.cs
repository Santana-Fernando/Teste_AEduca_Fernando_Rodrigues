using Application.Students.ViewModel;
using AutoMapper;
using Domain.Students.Entidades;

namespace Application.Mapping
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<StudentView, Student>()
                .ForMember(dest => dest.RA, opt => opt.MapFrom(src => src.RA))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF));
        }
    }
}
