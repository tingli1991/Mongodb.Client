using System;

namespace Mongodb.Client
{
    /// <summary>
    /// 实体类标示接口
    /// </summary>
    public class IEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
    }
}