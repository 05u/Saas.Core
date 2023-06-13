using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Respository
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitWork : IDisposable
    {
        /// <summary>
        /// 事务
        /// </summary>
        IDbContextTransaction Transaction { get; }

        /// <summary>
        /// 开启事务
        /// </summary>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// 释放
        /// </summary>
        Task DisposeAsync();
    }
}
