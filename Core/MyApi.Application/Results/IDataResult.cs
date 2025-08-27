using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Results
{
    public interface IDataResult<out T> : IResult
    {
        T? Data { get; }
    }
}
