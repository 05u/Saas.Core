using Saas.Core.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 数据实体基类
    /// </summary>
    public abstract class BaseEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public BaseEntity()
        {
            Id = SnowFlake.NewId();
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }



    }
}
