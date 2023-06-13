using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 家庭财务台账
    /// </summary>
    [Table("bus_home_financial_record")]
    public class BusHomeFinancialRecord : BaseEntity
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        public FinancialAccountType FinancialAccountType { get; set; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }

    }
}
