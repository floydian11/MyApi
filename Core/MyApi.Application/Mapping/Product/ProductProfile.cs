using AutoMapper;
using MyApi.Application.DTOs.Product;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyApi.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductListDto>().ReverseMap();
            CreateMap<Product, ProductCategoryDto>().ReverseMap();
            CreateMap<Product, ProductDetailDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductDetailInfoDto>().ReverseMap();
            CreateMap<Product, ProductDocumentDto>().ReverseMap();
            //CreateMap<Product, ProductResponseDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>()
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            //srcMember != null ise map et, null ise hiç dokunma.Yani kısmi update için güvenli mapping.
            CreateMap<Product, ProductResponseDto>()
     .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id).ToList()))
     .ForMember(dest => dest.ProductDocuments, opt => opt.MapFrom(src => src.ProductDocuments.Select(d => d.FilePath).ToList()));




        }
    }
}
