using Iris.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                _logger.LogError($"Mongodb 异步添加一条数据错误，数据为： {JsonConvert.SerializeObject(entity)}", e);
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
                _logger.LogError($"Mongodb 异步添加一条数据并返回ID错误，数据为： {JsonConvert.SerializeObject(entity)}", e);
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
                _logger.LogError($"Mongodb 异步添加多条数据错误，数据为： {JsonConvert.SerializeObject(entitys)}", e);
                return false;
            }

        }

        /// <summary>
        /// 异步根据条件获取总数
        /// 注：条件字段尽可能都是索引字段
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
                _logger.LogError($"Mongodb 异步根据条件获取总数错误，参数为： {JsonConvert.SerializeObject(filter)}", e);
                return 0;
            }
        }



        //public async Task<T> Aggregate()
        //{
        //    try
        //    {
        //        PipelineDefinition<User, UserBaseinfo>.Create();

        //        AggregateLookupOptions<User, UserBaseinfo> aggregateLookupOptions = new AggregateLookupOptions<User, UserBaseinfo>();
        //        _collection.Aggregate<T>(aggregateLookupOptions);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError("Mongodb 依据条件查询数据错误", e);
        //        return null;
        //    }
        //}


        /// <summary>
        /// 依据条件查询数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> @where = null)
        {
            try
            {
                return (await _collection.FindAsync(@where)).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 依据条件查询数据错误", e);
                return null;
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
                return (await _collection.FindAsync(@where)).ToList();
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
        public async Task<IEnumerable<T>> GetAsync(MongoModel<T> mongo)
        {
            try
            {
                var _ = GetFluent(mongo);
                return await _.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 依据查询条件查询所有数据 包含显示字段 排序错误", e);
                return null;
            }
        }

        /// <summary>
        ///  分页查询数据列表
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetByPageAsync(MongoModel<T> mongo, int pageIndex, int pageSize)
        {
            try
            {
                var _ = GetFluent(mongo);
                return await _.Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

            }
            catch (Exception e)
            {
                _logger.LogError("Mongodb 分页查询数据列表错误", e);
                return null;
            }
        }

        /// <summary>
        /// 获取查询相关 filter、sort、projection 等公共对象
        /// </summary>
        /// <param name="mongo"></param>
        /// <returns></returns>
        private IFindFluent<T, T> GetFluent(MongoModel<T> mongo)
        {
            var filter = mongo.FilterList.Any() ? mongo.Filter.And(mongo.FilterList) : null;
            var sort = mongo.SortList.Any() ? mongo.Sort.Combine(mongo.SortList) : null;
            var projection = mongo.ProjectionList.Any() ? mongo.Projection.Combine(mongo.ProjectionList) : null;

            IFindFluent<T, T> _ = null;
            if (filter != null)
                _ = _collection.Find(filter);
            else
                _ = _collection.Find(x => true);

            if (sort != null)
                _ = _.Sort(sort);

            if (projection != null)
                _ = _.Project<T>(projection);

            return _;
        }

    }
}
