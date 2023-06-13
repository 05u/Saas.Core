
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace Saas.Core.Data.Respository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        //数据库上下文
        private readonly MainDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public Repository(MainDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Queryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Queryable()
        {
            return _context.Set<T>().Where(x => !x.IsDeleted);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<string> InsertAsync(T entity)
        {
            var user = _currentUserService.GetCurrentUser();
            if (entity.CreateBy.IsBlank())
            {
                entity.CreateBy = user?.UserId;
            }
            if (entity.Id.IsBlank())
            {
                entity.Id = SnowFlake.NewId();
            }
            entity.CreateTime = DateTime.Now;
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public string Insert(T entity)
        {
            var user = _currentUserService.GetCurrentUser();
            if (entity.CreateBy.IsBlank())
            {
                entity.CreateBy = user?.UserId;
            }
            if (entity.Id.IsBlank())
            {
                entity.Id = SnowFlake.NewId();
            }
            entity.CreateTime = DateTime.Now;
            _context.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">实体集合</param>
        public async Task BatchInsertAsync(List<T> list)
        {
            var user = _currentUserService.GetCurrentUser();
            foreach (var entity in list)
            {
                if (entity.CreateBy.IsBlank())
                {
                    entity.CreateBy = user?.UserId;
                }
                if (entity.Id.IsBlank())
                {
                    entity.Id = SnowFlake.NewId();
                }
                entity.CreateTime = DateTime.Now;
            }
            await _context.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="properties">需要更新字段的表达式(可以不指定，默认全部更新)</param>
        /// <returns></returns>
        public async Task UpdateAsync(T entity, List<Expression<Func<T, object>>> notUpdateProperties = null, params Expression<Func<T, object>>[] properties)
        {
            var user = _currentUserService.GetCurrentUser();
            entity.UpdateBy = user?.UserId;
            entity.UpdateTime = DateTime.Now;
            var dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                try
                {
                    dbEntityEntry = _context.Attach(entity);
                }
                catch (Exception)
                {
                    T oldEntity = _context.Set<T>().Find(entity.Id);

                    _context.Entry(oldEntity).CurrentValues.SetValues(entity);
                    dbEntityEntry = _context.Entry(oldEntity);
                }

                dbEntityEntry.State = EntityState.Modified;
            }

            //主键,创建人,创建时间不更新
            dbEntityEntry.Property(x => x.Id).IsModified = false;
            dbEntityEntry.Property(x => x.CreateTime).IsModified = false;
            dbEntityEntry.Property(x => x.CreateBy).IsModified = false;
            dbEntityEntry.Property(x => x.IsDeleted).IsModified = false;


            //if (properties.AnyOne())
            //{
            //    foreach (var property in properties)
            //    {
            //        dbEntityEntry.Property(property).IsModified = true;
            //    }
            //    //修改时间和修改人，默认需要更新
            //    dbEntityEntry.Property(x => x.UpdateTime).IsModified = true;

            //}
            //else
            //{
            //    var notUpdateColumns = new List<string>
            //    {
            //        "Id",
            //        "CreateTime",
            //    };

            //    var updateColumns = new List<string>()
            //    {
            //        "UpdateTime",
            //    };
            //    foreach (var rawProperty in dbEntityEntry.Entity.GetType().GetTypeInfo().DeclaredProperties)
            //    {


            //        if ((!notUpdateColumns.Contains(rawProperty.Name) && !rawProperty.GetMethod.IsVirtual) || updateColumns.Contains(rawProperty.Name))
            //        {
            //            dbEntityEntry.Property(rawProperty.Name).IsModified = true;

            //        }
            //    }
            //}
            //if (notUpdateProperties != null && notUpdateProperties.Any())
            //{

            //    foreach (var property in notUpdateProperties)
            //    {
            //        dbEntityEntry.Property(property).IsModified = false;
            //    }
            //}

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="properties">需要更新字段的表达式(可以不指定，默认全部更新)</param>
        /// <returns></returns>
        public void Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            var user = _currentUserService.GetCurrentUser();
            entity.UpdateBy = user?.UserId;
            entity.UpdateTime = DateTime.Now;
            var dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;

            //主键和创建时间不更新
            dbEntityEntry.Property(x => x.Id).IsModified = false;
            dbEntityEntry.Property(x => x.CreateTime).IsModified = false;
            dbEntityEntry.Property(x => x.CreateBy).IsModified = false;
            dbEntityEntry.Property(x => x.IsDeleted).IsModified = false;

            //if (properties.AnyOne())
            //{
            //    foreach (var property in properties)
            //    {
            //        dbEntityEntry.Property(property).IsModified = true;
            //    }
            //    //修改时间和修改人，默认需要更新
            //    dbEntityEntry.Property(x => x.UpdateTime).IsModified = true;

            //}
            //else
            //{
            //    var notUpdateColumns = new List<string>
            //    {
            //        "Id",
            //        "CreateTime",
            //    };

            //    var updateColumns = new List<string>()
            //    {
            //        "UpdateTime",
            //    };
            //    foreach (var rawProperty in dbEntityEntry.Entity.GetType().GetTypeInfo().DeclaredProperties)
            //    {


            //        if ((!notUpdateColumns.Contains(rawProperty.Name) && !rawProperty.GetMethod.IsVirtual) || updateColumns.Contains(rawProperty.Name))
            //        {
            //            dbEntityEntry.Property(rawProperty.Name).IsModified = true;

            //        }
            //    }
            //}
            _context.SaveChanges();
        }




        /// <summary>
        /// 根据条件查询实体，不存在返回null
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Queryable().WhereIf(predicate != null, predicate).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> QueryableAll(Expression<Func<T, bool>> predicate)
        {
            return Queryable().WhereIf(predicate != null, predicate);
        }

        /// <summary>
        /// 批量更新所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件的谓语表达式</param>
        /// <param name="updateExpression">属性更新表达式</param>
        /// <returns>操作影响的行数</returns>
        public async Task<int> UpdateBatchAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression)
        {
            //var user = _currentUserService.GetCurrentUser();
            //updateExpression = updateExpression.And(c => new T { ModifyUserId = user.UserId, ModifyUserCardNo = user.CardNo, ModifyUserName = user.Name, UpdateTime = DateTime.Now });



            //updateExpression = c => new T { ModifyUserId = user.UserId, ModifyUserCardNo = user.CardNo, ModifyUserName = user.Name, UpdateTime = DateTime.Now };





            ////Check.NotNull(predicate, nameof(predicate));
            ////Check.NotNull(updateExpression, nameof(updateExpression));
            //var entity = new T { UpdateTime = DateTime.Now, ModifyUserId = user.UserId, ModifyUserCardNo = user.CardNo, ModifyUserName = user.Name };

            //var memberInitExpression = (MemberInitExpression)updateExpression.Body;
            //foreach (MemberAssignment item in memberInitExpression.Bindings)
            //{
            //    var property = entity.GetType().GetProperty(item.Member.Name);
            //    var experssion = (ConstantExpression)item.Expression;

            //    property.SetValue(entity, experssion.Value);
            //}
            //Expression<Func<T, T>> entityExpression = c => entity;

            return await Queryable().Where(predicate).UpdateAsync(updateExpression);
        }


        /// <summary>
        /// 批量更新所有符合特定条件的实体
        /// </summary>
        /// <param name="predicate">查询条件的谓语表达式</param>
        /// <param name="updateExpression"></param>
        /// <returns>操作影响的行数</returns>
        public int UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression)
        {
            //Check.NotNull(predicate, nameof(predicate));
            //Check.NotNull(updateExpression, nameof(updateExpression)) ;
            return Queryable().Where(predicate).Update(updateExpression);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BatchUpdateAsync(List<T> entities)
        {
            //var ids = entities.ConvertAll(c => c.Id);
            //var dbentities = _context.Set<T>().Where(c => ids.Contains(c.Id)).ToList();

            //dbentities.ForEach(item =>
            //{
            //    EntityEntry<T> entry = _context.Entry(item);

            //});
            var user = _currentUserService.GetCurrentUser();


            entities.ForEach(c =>
            {
                c.UpdateBy = user?.UserId;
                c.UpdateTime = DateTime.Now;

                var dbEntityEntry = _context.Entry(c);
                if (dbEntityEntry.State == EntityState.Detached)
                {
                    try
                    {
                        dbEntityEntry = _context.Attach(c);
                    }
                    catch (Exception)
                    {
                        T oldEntity = _context.Set<T>().Find(c.Id);

                        _context.Entry(oldEntity).CurrentValues.SetValues(c);
                        dbEntityEntry = _context.Entry(oldEntity);
                    }

                    dbEntityEntry.State = EntityState.Modified;
                }

                //主键,创建人,创建时间不更新
                dbEntityEntry.Property(x => x.Id).IsModified = false;
                dbEntityEntry.Property(x => x.CreateTime).IsModified = false;
                dbEntityEntry.Property(x => x.CreateBy).IsModified = false;
                dbEntityEntry.Property(x => x.IsDeleted).IsModified = false;


                ////修改时间和修改人，默认需要更新
                //dbEntityEntry.Property(x => x.UpdateTime).IsModified = true;
                //var notUpdateColumns = new List<string>
                //{
                //    "Id",
                //    "CreateTime",
                //};
                //var updateColumns = new List<string>()
                //{
                //    "UpdateTime",
                //};
                //foreach (var rawProperty in dbEntityEntry.Entity.GetType().GetTypeInfo().DeclaredProperties)
                //{
                //    if ((!notUpdateColumns.Contains(rawProperty.Name) && !rawProperty.GetMethod.IsVirtual) || updateColumns.Contains(rawProperty.Name))
                //    {
                //        if (rawProperty.Name == "ProductType")
                //        {
                //            var s = dbEntityEntry.Property(rawProperty.Name);
                //        }
                //        dbEntityEntry.Property(rawProperty.Name).IsModified = true;

                //    }
                //}

            });
            await _context.SaveChangesAsync();
        }





        /// <summary>
        /// 删除指定主键的数据(逻辑删除)
        /// </summary>
        /// <param name="keyId">主键Id</param>
        /// <returns></returns>
        public async Task DeleteAsync(string keyId)
        {
            if (keyId.IsBlank())
            {
                return;
            }
            var user = _currentUserService.GetCurrentUser();

            var entity = await Queryable().Where(x => x.Id == keyId).FirstOrDefaultAsync();
            entity.UpdateTime = DateTime.Now;
            entity.UpdateBy = user?.UserId;
            var dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.Property(x => x.IsDeleted).IsModified = true;
            dbEntityEntry.Property(x => x.UpdateTime).IsModified = true;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 批量删除(逻辑删除)
        /// </summary>
        /// <param name="keyIds">主键Id集合</param>
        /// <returns></returns>
        public async Task DeleteAsync(IList<string> keyIds)
        {
            if (keyIds.AnyOne())
            {
                var user = _currentUserService.GetCurrentUser();

                var entityList = await Queryable().Where(x => keyIds.Contains(x.Id)).ToListAsync();
                foreach (var entity in entityList)
                {
                    entity.IsDeleted = true;
                    entity.UpdateTime = DateTime.Now;
                    entity.UpdateBy = user?.UserId;

                }
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 根据条件删除实体(软删除)
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<bool> SoftDeleteAsync(Expression<Func<T, bool>> predicate)
        {

            var user = _currentUserService.GetCurrentUser();

            var entityList = await Queryable().Where(predicate).ToListAsync();
            foreach (var entity in entityList)
            {
                entity.IsDeleted = true;
                entity.UpdateTime = DateTime.Now;
                entity.UpdateBy = user?.UserId;

            }
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 根据条件删除实体(物理删除)
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entityList = await Queryable().Where(predicate).ToListAsync();
            _context.RemoveRange(entityList);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 根据主键查询实体，如果没有找到，则抛出异常信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public async Task<T> FindAsync(object keyValue)
        {
            var entity = await _context.Set<T>().FindAsync(keyValue);
            if (entity == null)
            {
                throw new BusinessException($"找不到此数据 ({keyValue})实体:{typeof(T)?.Name}");
            }
            return entity;
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = _context.Set<T>().Single(predicate);
            if (entity == null)
            {
                throw new BusinessException("没有找到对应的数据");
            }
            return entity;
        }




        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Queryable().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 查询不跟踪实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FindAsNoTrackingAsync(Expression<Func<T, bool>> predicate)
        {
            return await Queryable().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 查询不跟踪实体
        /// </summary>
        /// <returns></returns>
        public async Task<T> FindAsNoTrackingAsync(string id)
        {

            var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null)
            {
                throw new BusinessException("没有找到对应的数据");
            }
            return entity;
        }

        /// <summary>
        /// 根据条件判断是否存在相同的数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await Queryable().AsNoTracking().AnyAsync(predicate);
        }


        /// <summary>
        /// 根据条件判断是否存在相同的数据
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Queryable().AsNoTracking().Any(predicate);
        }

        #region 原生SQL
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IQueryable<T> FromSql(string sql)
        {
            return _context.Set<T>().FromSqlRaw(sql);
        }

        /// <summary>
        /// 查询(自定义实体)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IQueryable<T2> FromSql<T2>(string sql) where T2 : class
        {
            return _context.Set<T2>().FromSqlRaw(sql);
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            return _context.Database.ExecuteSqlRaw(sql);
        }

        /// <summary>
        /// 增删改（异步）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql);
        }
        #endregion

    }

}
