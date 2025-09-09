using AutoMapper;
using MyApi.Application.DTOs.Order;
using MyApi.Application.DTOs.Product;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<Order, OrderListDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.Select(d => d.ProductId)));
        }
    }
}
