using MyApi.Application.Repositories;
using MyApi.Domain.Entities;
using MyApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepositorty
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}

//IProductRepositorty → interface, DI container’ın hangi türü inject edeceğini biliyor.

//Yani ProductRepository hem base class’taki generic metotları kullanıyor hem de IProductRepository olarak DI’ye kaydediliyor.