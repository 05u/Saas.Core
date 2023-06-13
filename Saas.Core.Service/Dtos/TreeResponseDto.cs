using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 树状结构模型
    /// </summary>
    public class TreeResponseDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 唯一key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 点击跳转的Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ImagePath { get; set; }


        public int Order { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeResponseDto> Children { get; set; }
    }

    /// <summary>
    /// ztree模型
    /// </summary>
    public class TreeDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 父类ID
        /// </summary>
        public string PId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Open { get; set; }

        /// <summary>
        /// 是否是父节点
        /// </summary>
        public bool IsParent { get; set; }
    }

    /// <summary>
    /// 下拉框树状结构模型
    /// </summary>
    public class SelectTreeResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// PId
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Label
        /// </summary>
        public string Label { get; set; }
    }

    /// <summary>
    /// 下拉框普通结构模型
    /// </summary>
    public class SelectResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Label
        /// </summary>
        public string Text { get; set; }
    }
}
