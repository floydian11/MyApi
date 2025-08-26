using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Services.Abstract;
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
       
        public OrderService(IUnitOfWork unitOfWork)
            : base(unitOfWork.Orders, unitOfWork)
        {
        }
    }
}
