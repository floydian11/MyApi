using MyApi.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository'ler
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }

        // Normal commit (tek işlem / repo için)
        Task<int> CommitAsync();

        // Transaction gerektiren çoklu işlem
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
