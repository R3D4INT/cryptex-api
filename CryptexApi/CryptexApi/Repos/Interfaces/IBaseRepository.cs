using CryptexApi.Models;
using System.Linq.Expressions;
using CryptexApi.Models.Base;

namespace CryptexApi.Repos.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<Result<IEnumerable<T>>> GetListByConditionAsync(Expression<Func<T, bool>> condition);
        Task<Result<T>> GetSingleByConditionAsync(Expression<Func<T, bool>> condition);
        Task<Result<T>> AddAsync(T item);
        Task<Result<bool>> UpdateAsync(T item, Expression<Func<T, bool>> condition);
        Task<Result<bool>> DeleteAsync(Expression<Func<T, bool>> condition);
    }
}
