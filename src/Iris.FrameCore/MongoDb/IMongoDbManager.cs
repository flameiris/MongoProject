using Iris.Models.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iris.FrameCore.MongoDb
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
        Task<IEnumerable<T>> GetByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, SortDefinition<T> sort = null);
    }
}
