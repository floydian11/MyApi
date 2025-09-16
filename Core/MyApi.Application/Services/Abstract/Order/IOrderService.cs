using MyApi.Application.DTOs.ARServices.Order;
using MyApi.Application.DTOs.Pagination;
using MyApi.Application.Results;
using MyApi.Application.Services.Concrete;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract
{
    public interface IOrderService : IServiceBase<Order>
    {
        Task<IDataResult<OrderResponseDto?>> AddOrderAsync(OrderCreateDto orderDto);
        Task<IDataResult<OrderResponseDto?>> UpdateOrderAsync(OrderUpdateDto orderDto);
        Task<IResult> DeleteOrderAsync(Guid id);
        Task<IDataResult<OrderResponseDto>> GetOrderByIdAsync(Guid orderId);
        Task<IDataResult<PagedResultDto<OrderListDto>>> GetAllOrdersAsync(PaginationRequestDto request);

        // OrderItem ekleme/güncelleme/silme
        Task<IDataResult<OrderItemResponseDto>> AddOrderItemAsync(Guid orderId, OrderItemCreateDto itemDto);
        Task<IDataResult<OrderItemResponseDto>> UpdateOrderItemAsync(OrderItemUpdateDto itemDto);
        Task<IDataResult<bool>> DeleteOrderItemAsync(Guid orderItemId);
    }
}
