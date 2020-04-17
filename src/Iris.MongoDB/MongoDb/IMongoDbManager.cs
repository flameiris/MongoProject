using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Iris.MongoDB
{
    public interface IMongoDbManager<T> where T : class, IBaseModel
    {
        /// <summary>
        /// 添加一項
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddAsync(T entity);

        Task<long> CountAsync(FilterDefinition<T> filter = null);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> @where = null);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> @where = null);
        Task<IEnumerable<T>> GetAsync(MongoModel<T> mongo);
        Task<IEnumerable<T>> GetByPageAsync(MongoModel<T> mongo, int pageIndex, int pageSize);
    }
}
