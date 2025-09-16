using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.DTOs.ARServices.Order;
using MyApi.Application.DTOs.Pagination;
using MyApi.Application.DTOs.ARServices.Product;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Results;
using MyApi.Application.Services.Abstract;
using MyApi.Application.Utilities;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Concrete
{
    public class OrderService : ServiceBase<Order>, IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork.Orders, unitOfWork, mapper)
        {
            _orderRepository = orderRepository;
        }

       

        public async Task<IDataResult<OrderResponseDto?>> AddOrderAsync(OrderCreateDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // DTO → Entity
                var order = _mapper.Map<Order>(dto);

                // OrderItem’lar zaten DTO içinde geldi, AutoMapper ile maplenir
                if (dto.OrderItems.Any())
                {
                    foreach (var itemDto in dto.OrderItems)
                    {
                        var orderItem = _mapper.Map<OrderItem>(itemDto);
                        order.Items.Add(orderItem);
                    }
                }

                // Eğer ileride ek belge, dosya vb. olacaksa burada ekleyebilirsin
                // Örn: invoice, file attachments vs.

                // Veritabanına ekle
                await _repository.AddAsync(order);
                await _unitOfWork.CommitAsync();

                // Response DTO map et
                return _mapper.Map<OrderResponseDto>(order);

            },
                successMessage: "Sipariş başarıyla eklendi.",
                errorMessage: "Sipariş eklenemedi.");
        }

        public async Task<IDataResult<OrderResponseDto?>> UpdateOrderAsync(OrderUpdateDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Order var mı?
                var order = await _repository.GetByIdAsync(dto.Id);
                if (order is null)
                    throw new KeyNotFoundException("Sipariş bulunamadı.");

                // OrderItem'lara dokunma, sadece ana bilgileri güncelle
                order.CustomerName = dto.CustomerName;
                order.PaymentMethod = dto.PaymentMethod;
                order.OrderDate = dto.OrderDate;
                order.Notes = dto.Notes;

                // Güncelle
                _repository.Update(order);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<OrderResponseDto>(order);
            },
            successMessage: "Sipariş başarıyla güncellendi.",
            errorMessage: "Sipariş güncellenemedi.");
        }

        public async Task<IResult> DeleteOrderAsync(Guid id)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var order = await _repository.GetByIdAsync(id);
                if (order is null)
                    throw new KeyNotFoundException("Silinecek sipariş bulunamadı.");

                await _repository.DeleteAsync(order);
                await _unitOfWork.CommitAsync();

                return true; // ServiceExecutor bunu SuccessResult’a çeviriyor
            },
            successMessage: "Sipariş başarıyla silindi.",
            errorMessage: "Sipariş silinemedi.");
        }

        public async Task<IDataResult<PagedResultDto<OrderListDto>>> GetAllOrdersAsync(PaginationRequestDto request)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Order tablosu + Items + Product join
                var query = _repository
                    .GetAll()
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product);

                // Toplam kayıt sayısı
                var totalCount = await query.CountAsync();

                // Pagination uygulanmış liste
                var orders = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // DTO mapleme
                var dto = _mapper.Map<List<OrderListDto>>(orders);

                // PagedResultDto oluştur
                var pagedResult = new PagedResultDto<OrderListDto>
                {
                    Items = dto,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize
                };

                return pagedResult;
            },
            successMessage: "Siparişler başarıyla listelendi.",
            errorMessage: "Siparişler listelenemedi.");
        }

        public async Task<IDataResult<OrderResponseDto>> GetOrderByIdAsync(Guid orderId)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var order = await _repository
                    .GetWhere(o => o.Id == orderId)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync();

                if (order is null)
                    throw new KeyNotFoundException("Sipariş bulunamadı.");

                var dto = _mapper.Map<OrderResponseDto>(order);
                return dto;
            },
            successMessage: "Sipariş başarıyla getirildi.",
            errorMessage: "Sipariş getirilemedi.");
        }

        public async Task<IDataResult<OrderItemResponseDto>> AddOrderItemAsync(Guid orderId, OrderItemCreateDto itemDto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Order kontrolü
                var order = await _orderRepository
                    .GetWhere(o => o.Id == orderId)
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync();
                if (order is null)
                    throw new Exception("Sipariş bulunamadı.");

                // DTO → Entity map
                var itemEntity = _mapper.Map<OrderItem>(itemDto);
                itemEntity.OrderId = orderId;

                // Order'a ekle
                order.Items.Add(itemEntity);

                await _unitOfWork.CommitAsync();

                // Response DTO map
                var response = _mapper.Map<OrderItemResponseDto>(itemEntity);

                return response;
            },
            successMessage: "Siparişe ürün başarıyla eklendi.",
            errorMessage: "Siparişe ürün eklenemedi.");
        }


        public async Task<IDataResult<OrderItemResponseDto>> UpdateOrderItemAsync(OrderItemUpdateDto itemDto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Order'ları item'larıyla birlikte çekiyoruz
                var orders = await _orderRepository.GetAll()
                    .Include(o => o.Items)
                    .ToListAsync();

                // Item'ı bul
                var order = orders.FirstOrDefault(o => o.Items.Any(i => i.Id == itemDto.Id));
                if (order is null)
                    throw new Exception("Sipariş kalemi bulunamadı.");

                var item = order.Items.First(i => i.Id == itemDto.Id);

                // Güncelle
                item.Quantity = itemDto.Quantity;
                item.Discount = itemDto.Discount;

                 _orderRepository.Update(order);
                await _unitOfWork.CommitAsync();

                var response = _mapper.Map<OrderItemResponseDto>(item);
                return response;
            },
            successMessage: "Sipariş kalemi başarıyla güncellendi.",
            errorMessage: "Sipariş kalemi güncellenemedi.");
        }


        public async Task<IDataResult<bool>> DeleteOrderItemAsync(Guid orderItemId)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Order'ı item'larıyla birlikte çek
                var orders = await _orderRepository.GetAll()
                    .Include(o => o.Items)
                    .ToListAsync();

                // Silinecek item'ı bul
                var order = orders.FirstOrDefault(o => o.Items.Any(i => i.Id == orderItemId));
                if (order == null)
                    throw new Exception("Sipariş kalemi bulunamadı.");

                var item = order.Items.First(i => i.Id == orderItemId);

                // Koleksiyondan çıkar → Cascade Delete sayesinde DB'den de silinir
                order.Items.Remove(item);

                 _orderRepository.Update(order);
                await _unitOfWork.CommitAsync();

                return true;
            },
            successMessage: "Sipariş kalemi başarıyla silindi.",
            errorMessage: "Sipariş kalemi silinemedi.");
        }



    }
}
