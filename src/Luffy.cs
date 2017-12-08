using System;
using System.Threading.Tasks;

namespace Luffy
{
    public class Luffy : ILuffy
    {
        public Task Execute(Action action)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(Func<Task> func)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            throw new NotImplementedException();
        }
    }
}