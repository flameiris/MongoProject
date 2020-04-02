using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Iris.FrameCore.MongoDb
{
    public class MongoModel<T>
    {
        public FilterDefinitionBuilder<T> Filter { get; } = new FilterDefinitionBuilder<T>();
        public IndexKeysDefinitionBuilder<T> IndexKeys { get; } = new IndexKeysDefinitionBuilder<T>();
        public ProjectionDefinitionBuilder<T> Projection { get; } = new ProjectionDefinitionBuilder<T>();
        public SortDefinitionBuilder<T> Sort { get; } = Builders<T>.Sort;
        public UpdateDefinitionBuilder<T> Update { get; } = new UpdateDefinitionBuilder<T>();



        public List<FilterDefinition<T>> FilterList { get; set; } = new List<FilterDefinition<T>>();
        public List<IndexKeysDefinition<T>> IndexKeysList { get; } = new List<IndexKeysDefinition<T>>();
        public List<ProjectionDefinition<T>> ProjectionList { get; } = new List<ProjectionDefinition<T>>();
        public List<SortDefinition<T>> SortList { get; } = new List<SortDefinition<T>>();
        public List<UpdateDefinition<T>> UpdateList { get; } = new List<UpdateDefinition<T>>();


        public void Ascending(FieldDefinition<T> field)
        {
            SortList.Add(Sort.Ascending(field));
        }
        public void Descending(FieldDefinition<T> field)
        {
            SortList.Add(Sort.Descending(field));
        }

        public void Where(Expression<Func<T, bool>> expression)
        {
            FilterList.Add(Filter.Where(expression));
        }

        public void Or(Expression<Func<T, bool>> expression)
        {
            FilterList.Add(Filter.Or(expression));
        }

        public void Include(params string[] fields)
        {
            ProjectionList.AddRange(fields.ToList().Select(x => Projection.Include(x)));
        }

        public void Exclude(params string[] fields)
        {
            ProjectionList.AddRange(fields.ToList().Select(x => Projection.Exclude(x)));
        }
    }
}
