﻿using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repository;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    /// <summary>
    /// 基础仓储实现
    /// </summary>
    public class RepositoryAsync<TEntity, Tkey> : IRepositoryAsync<TEntity, Tkey> where TEntity : BaseEntity
    {
        /// <summary>
        /// 数据上下文
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// 模型
        /// </summary>
        public virtual DbSet<TEntity> _dbSet => _dbContext.Set<TEntity>();

        public RepositoryAsync(DbContext context)
        {
            _dbContext = context;
        }

        #region 查询

        public IQueryable<TEntity> GetQuery() => _dbSet.AsQueryable();

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await GetQuery().Where(expression).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> FindByIdAsync(Tkey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await GetQuery().SingleAsync(expression);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await GetQuery().FirstOrDefaultAsync(expression);
        }

        public async Task<long> Count(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.CountAsync(expression);
        }

        #endregion


        #region 新增
        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task BatchInsertAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion


        #region 修改
        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }


        //public async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> expression)
        //{
        //    //return await Entity.BatchUpdateAsync();
        //}

        #endregion


        #region 删除

        public void DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.Where(expression).BatchDeleteAsync();
        }


        #endregion
    }
}
