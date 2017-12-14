using System;
using System.Threading.Tasks;

namespace Luffy
{
    public interface IExecutionOperation
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> func);
    }
}