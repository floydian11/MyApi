using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Utilities
{
    public static class ServiceExecutor
    {
        public static async Task<IDataResult<T>> ExecuteAsync<T>(Func<Task<T>> func, string successMessage, string? errorMessage)
        {
            try
            {
                var data = await func();
                return new SuccessDataResult<T>(data, successMessage);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<T>(default, $"{errorMessage}: {ex.Message}");
            }
        }
    }
}
