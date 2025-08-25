using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(AppDbContext context, ICategoryRepository categories, IProductRepository products, IOrderRepository orders)
        {
            _context = context;
            Categories = categories;
            Products = products;
            Orders = orders;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // ✅ Merkezileştirilmiş transaction metodu - bunu çoklı işlemler için kullanacağız.
        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();          // Servisteki işlemler buraya gelir
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
