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
            // Create - Update
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems)); // koleksiyonu buna göre otomatik map et demek
            
            CreateMap<OrderItemCreateDto, OrderItem>();

            CreateMap<OrderUpdateDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.Ignore()); //update'de items'ı almayacağız. ınun için başka metot olacak. item ekle sil güncelle gibi
            
            CreateMap<OrderItemUpdateDto, OrderItem>();

            // Response - List
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));
            
            CreateMap<Order, OrderListDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

            
            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price));
        }
    }
}
