using System;
using System.Threading.Tasks;

namespace LuffyCore
{
    public interface IExecutionOperation
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> func);
    }
}