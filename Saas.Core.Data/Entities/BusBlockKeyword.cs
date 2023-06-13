using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 封锁关键词
    /// </summary>
    [Table("bus_block_keyword")]
    public class BusBlockKeyword : BaseEntity
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

    }
}
