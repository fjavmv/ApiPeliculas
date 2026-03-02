using ApiImages.Models;
using ApiImages.Models.Dtos.Category.RequestDto;
using ApiImages.Models.Dtos.Category.ResponseDto;
using ApiImages.Models.Dtos.Image.RequestDto;
using ApiImages.Models.Dtos.Image.ResponseDto;
using AutoMapper;

namespace ApiImages.Mappers
{
   public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //Mapper para categorias
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryDetailResponse>().ReverseMap();
            CreateMap<Category, DeleteCategoryResponse>().ReverseMap();

            //Mapper para Imagenes
            CreateMap<Image, ImageDto>().ReverseMap();
            CreateMap<CreateImageRequest, Image>().ReverseMap();
            CreateMap<Image, ImageResponse>().ReverseMap();
            CreateMap<Image, UpdateImageRequest>().ReverseMap();
            CreateMap<Image, ImagePatchDto>().ReverseMap();

            // Mapping para el upload request (si es necesario)
            // Esto puede ser útil si quieres mapear directamente
            CreateMap<ImageUploadRequest, CreateImageRequest>()
                .ForMember(dest => dest.Size, opt => opt.Ignore()) // Se establecerá manualmente
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.File.ContentType));

        }
    }
}
