using AutoMapper;
using MyApi.Application.DTOs.Category;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryActiveStatusDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>();
            CreateMap<Category, CategoryDeleteDto>();
            CreateMap<Category, CategoryUpdateDto>();
            CreateMap<Category, CategoryReadDto>().ReverseMap();            
            CreateMap<Category, CategoryWithProductsDto>().ReverseMap();
        }        
    }
}
