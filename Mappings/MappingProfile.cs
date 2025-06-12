using AutoMapper;
using FinTrackHub.Entities;
using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Account, AccountDto>()
            //    .ForMember(dest => dest.AccountGroupName,
            //        opt => opt.MapFrom(src => src.AccountGroup != null ? src.AccountGroup.AccountGroupName : string.Empty))
            //    .ForMember(dest => dest.AccountGroupTypeName,
            //        opt => opt.MapFrom(src => src.AccountGroup != null && src.AccountGroup.AccountGroupType != null
            //                                 ? src.AccountGroup.AccountGroupType.AccountGroupTypeName : string.Empty))
            //    .ForMember(dest => dest.CreatedBy,
            //        opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.FullName : string.Empty))
            //    .ForMember(dest => dest.UpdatedBy,
            //        opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.FullName : string.Empty));

            CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.AccountGroupName,
                opt => opt.MapFrom(src => src.AccountGroup != null ? src.AccountGroup.AccountGroupName : string.Empty))
            .ForMember(dest => dest.AccountGroupTypeName,
                opt => opt.MapFrom(src => src.AccountGroup != null && src.AccountGroup.AccountGroupType != null
                                         ? src.AccountGroup.AccountGroupType.AccountGroupTypeName : string.Empty))
            .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.FullName : string.Empty))
            .ForMember(dest => dest.UpdatedBy,
                opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.FullName : string.Empty))
            .ReverseMap(); // <-- Add this to allow AccountDto → Account mapping

        }

    }

}
