using System;
using System.Threading.Tasks;

namespace Globe.BusinessLogic.Repositories
{
    public interface IWriteRepository<T>
        where T : class
    {
        void Insert(T entity);
        void Update(T entity);
    }

    public interface IAsyncWriteRepository<T>
        where T : class
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
