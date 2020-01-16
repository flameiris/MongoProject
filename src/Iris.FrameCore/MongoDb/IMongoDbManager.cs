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

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> @where = null);

    }
}
