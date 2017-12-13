using System;
using System.Threading.Tasks;

namespace Luffy
{
    public interface IExecutionOperation
    {
        Task ExecuteAsync(Func<Task> func);
        Task<T> ExecuteAsync<T>(Func<Task<T>> func);
        Task Execute(Action action);
    }
}