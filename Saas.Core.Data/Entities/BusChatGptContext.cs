using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// ChatGPT上下文管理
    /// </summary>
    [Table("bus_chat_gpt_context")]
    public class BusChatGptContext : BaseEntity
    {
        /// <summary>
        /// 第三方平台标识(如QQ号,wxid等)
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 可用次数
        /// </summary>
        public int AvailableCount { get; set; }

        /// <summary>
        /// 已用次数
        /// </summary>
        public int UsedCount { get; set; }

    }
}
