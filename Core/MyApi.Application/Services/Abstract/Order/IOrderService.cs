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

    }
}
