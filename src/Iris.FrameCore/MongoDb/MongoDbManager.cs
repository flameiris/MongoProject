using Iris.Models.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iris.FrameCore.MongoDb
{
    public class MongoDbManager<T> : IMongoDbManager<T> where T : class, IBaseModel
    {
        private readonly ILogger _logger;

        protected MongoDbContext _context;
        protected readonly IMongoCollection<T> _collection;

        /// <summary>
        /// 实例化Mongo链接
        /// </summary>
        /// <param name="settings"></param>
        public MongoDbManager(IOptions<MongodbOptions> settings, ILogger<MongoDbManager<T>> logger)
        {
            _logger = logger;

            _context = new MongoDbContext(settings);
            _collection = _context.GetCollection<T>();
        }



        /// <summary>
        /// 异步添加一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 异步添加一条数据错误", e);
                return false;
            }
        }

        /// <summary>
        /// 异步添加一条数据并返回ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> AddRetIDAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                return entity.Id.ToString();
            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 异步添加一条数据并返回ID错误", e);
                return null;
            }
        }

        /// <summary>
        /// 异步添加多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeAsync(List<T> entitys)
        {
            try
            {
                await _collection.InsertManyAsync(entitys);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 异步添加多条数据错误", e);
                return false;
            }

        }

        /// <summary>
        /// 异步根据条件获取总数
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public async Task<long> CountAsync(FilterDefinition<T> filter = null)
        {
            try
            {
                return await _collection.CountDocumentsAsync(filter ?? FilterDefinition<T>.Empty);
            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 异步根据条件获取总数错误", e);
                return 0;
            }
        }




        /// <summary>
        /// 依据条件查询数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> @where = null)
        {
            try
            {
                var task = await _collection.FindAsync(@where);
                return task.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 依据条件查询数据错误", e);
                return null;
            }
        }

        /// <summary>
        /// 依据查询条件查询所有数据 包含显示字段 排序
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="field">查询显示字段</param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync(FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null)
        {
            try
            {
                //不指定查询字段
                if (field == null || field.Length == 0)
                {
                    if (filter == null)
                    {
                        if (sort == null) return await _collection.Find(_ => true).ToListAsync();
                        //进行排序
                        return await _collection.Find(_ => true).Sort(sort).ToListAsync();
                    }

                    if (sort == null) return await _collection.Find(filter).ToListAsync();
                    //进行排序
                    return await _collection.Find(filter).Sort(sort).ToListAsync();
                }

                #region 指定查询字段
                //指定查询字段
                var fieldList = new List<ProjectionDefinition<T>>();

                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                if (sort == null) return await _collection.Find(filter).Project<T>(projection).ToListAsync();
                //排序查询
                return await _collection.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
                #endregion

            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 依据查询条件查询所有数据 包含显示字段 排序错误", e);
                return null;
            }
        }

    }
}
