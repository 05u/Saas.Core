using Microsoft.EntityFrameworkCore.Storage;
using Saas.Core.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Respository
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitWork : IUnitWork
    {

        /// <summary>
        /// 数据库连接
        /// </summary>
        private MainDbContext _context;
        //private readonly ICapPublisher _capBus;

        /// <summary>
        /// 工作单元
        /// </summary>
        /// <param name="context"></param>
        public UnitWork(MainDbContext context)
        {
            _context = context;
            //_capBus = capBus;
        }

        /// <summary>
        /// 事务
        /// </summary>
        public IDbContextTransaction Transaction { get; private set; }

        /// <summary>
        /// 开启事务
        /// </summary>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (Transaction == null)
            {
                Transaction = await _context.Database.BeginTransactionAsync();
                //if (_capBus == null)
                //{
                //Transaction = await _context.Database.BeginTransactionAsync();
                //}
                //else
                //{
                //    Transaction =  _context.Database.BeginTransaction(_capBus, false);
                //}
            }
            return Transaction;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public async Task CommitAsync()
        {
            await Transaction?.CommitAsync();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public async Task RollbackAsync()
        {
            if (Transaction != null)
            {
                await Transaction.RollbackAsync();
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public async Task DisposeAsync()
        {
            if (Transaction != null)
            {
                await Transaction.DisposeAsync();
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Transaction?.Dispose();
        }
    }
}
