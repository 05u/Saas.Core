using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 通知任务类型
    /// </summary>
    public enum NoticeTaskType
    {
        [Description("一次")]
        Once = 0,

        [Description("每日")]
        EveryDay = 1,

        [Description("每周")]
        EveryWeek = 2,

        [Description("每月")]
        EveryMonth = 3,

        [Description("每季度")]
        EveryQuarter = 4,

        [Description("每半年")]
        EveryHalfYear = 5,

        [Description("每年")]
        EveryYear = 6,
    }
}
