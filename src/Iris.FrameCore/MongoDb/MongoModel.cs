using MongoDB.Driver;
using System;
using System.Collections.Generic;
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
    }
}
