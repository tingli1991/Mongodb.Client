using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mongodb.Client
{
    /// <summary>
    /// 数据库管理类
    /// </summary>
    public class MongodbManager<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        private readonly string ConfigName = "mongoConnStrings";

        /// <summary>
        /// 服务器地址和端口
        /// </summary> 
        protected IMongoCollection<TEntity> Collection { get; private set; }

        /// <summary>
        /// 构造实例
        /// </summary>
        public MongodbManager()
        {
            var database = GetDatabase(ConfigName);
            Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <summary>
        /// 构造实例
        /// </summary>
        /// <param name="configName">配置文件名称</param>
        public MongodbManager(string configName)
        {
            ConfigName = configName;
            var database = GetDatabase(ConfigName);
            Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="configName">配置文件名称</param>
        /// <returns></returns>
        private static IMongoDatabase GetDatabase(string configName)
        {
            var connectString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertOneAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await Collection.InsertManyAsync(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task ReplaceOneAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> GetList()
        {
            return await Collection.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        public async Task<long> GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.CountAsync(predicate);
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }
    }
}