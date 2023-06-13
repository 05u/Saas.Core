using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    public class ClientHeartbeatInput
    {
        /// <summary>
        /// 客户端名称
        /// </summary>
        [Required]
        public string ClientName { get; set; }

    }

    public class ClientHeartbeatOutput : BusRemoteCommand
    {

    }

    public class SendRemoteCommandInput
    {
        /// <summary>
        /// 客户端名称
        /// </summary>
        [Required]
        public string ClientName { get; set; }

        /// <summary>
        /// 远程指令类型
        /// </summary>
        [Required]
        public ActionType ActionType { get; set; }
    }

    /// <summary>
    /// 分页查询入参
    /// </summary>
    public class SearchInput : BaseFilter
    {

    }

}