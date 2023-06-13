namespace Saas.Core.Infrastructure.Attributes
{


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ToolTipsAttribute : Attribute
    {

        /// <summary>
        /// 初始化新的实例
        /// </summary>
        /// <param name="description">说明内容</param>
        public ToolTipsAttribute(string text) => Text = text;

        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Text { get; }
    }
}
