using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    public class ClientHeartbeatInput
    {
        /// <summary>
        /// �ͻ�������
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
        /// �ͻ�������
        /// </summary>
        [Required]
        public string ClientName { get; set; }

        /// <summary>
        /// Զ��ָ������
        /// </summary>
        [Required]
        public ActionType ActionType { get; set; }
    }

    /// <summary>
    /// ��ҳ��ѯ���
    /// </summary>
    public class SearchInput : BaseFilter
    {

    }

}