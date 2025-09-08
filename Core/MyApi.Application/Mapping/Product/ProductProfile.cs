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
            // Product -> ProductResponseDto
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id)))
                .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)))
                .ForMember(dest => dest.ProductDocuments, opt => opt.MapFrom(src => src.ProductDocuments));

            // Product -> ProductDetailDto
            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.ProductDetail, opt => opt.MapFrom(src => src.ProductDetail))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.ProductDocuments));
            
            // Nested DTOs
            CreateMap<ProductDetail, ProductDetailInfoDto>();
            CreateMap<ProductDocument, ProductDocumentDto>();
            CreateMap<Category, ProductCategoryDto>();

            CreateMap<Product, ProductListDto>()
                .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)))
                .ForMember(dest => dest.DocumentPaths, opt => opt.MapFrom(src => src.ProductDocuments.Select(d => d.FilePath)));


            CreateMap<ProductCreateDto, Product>()
               .ForMember(dest => dest.ProductDocuments, opt => opt.Ignore()) // Dosyaları servis içinde manuel handle edeceğiz
               .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Kategorileri manuel ekleyeceğiz
               .ForMember(dest => dest.ProductDetail, opt => opt.MapFrom(src => new ProductDetail
                    {
                       Manufacturer = src.Manufacturer,
                       TechnicalSpecs = src.TechnicalSpecs,
                       WarrantyPeriodInMonths = src.WarrantyPeriodInMonths
                    }));

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.ProductDocuments, opt => opt.Ignore()) // Dosyaları servis içinde manuel handle edeceğiz
                .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Kategorileri manuel ekleyeceğiz
                .ForMember(dest => dest.ProductDetail, opt => opt.MapFrom(src => new ProductDetail
                {
                    Manufacturer = src.Manufacturer ?? string.Empty,
                    TechnicalSpecs = src.TechnicalSpecs ?? string.Empty,
                    WarrantyPeriodInMonths = src.WarrantyPeriodInMonths ?? 0
                }));

        }
    }
}
