using HxBlogs.Model;
using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.DAL
{
    public abstract class BaseDal<T> where T:class,new()
    {
        internal virtual DbContext Context
        {
            get { return DbFactory.GetDbSession().DbContext; }
        }
        #region 查询出单条数据
        public virtual T GetEntity(Expression<Func<T, bool>> lambda)
        {
            var result = Context.Set<T>().Where(lambda);
            return result.FirstOrDefault();
        }

        public virtual T QueryEntityNoTrack(Expression<Func<T, bool>> lambda)
        {
            var result = Context.Set<T>()
                .AsNoTracking()
                .Where(lambda);
            return result.FirstOrDefault();
        }
        public T GetEntityByID(object id)
        {
            return Context.Set<T>().Find(id);
        }

        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="condition">where条件</param>
        /// <returns></returns>
        public T GetEntityBySql(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition)) return null;
            string tableName = nameof(T);
            TableAttribute table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            if (table != null && !string.IsNullOrEmpty(table.Name))
            {
                tableName = table.Name;
            }
            string sql = string.Format(@"select * from {0} where {1}", tableName, condition);
            return Context.Database.SqlQuery<T>(sql).FirstOrDefault();
        }
        #endregion
        #region 查询出数据的集合
       
        public virtual IQueryable<T> GetEntities(Expression<Func<T, bool>> lambda)
        {
            var result = Context.Set<T>().Where(lambda);
            return result;
        }
      
        public virtual IQueryable<T> GetEntitiesNoTrack(Expression<Func<T, bool>> lambda)
        {
            var result = Context.Set<T>()
                .AsNoTracking()
                .Where(lambda);
            return result;
        }
        #endregion

        #region 分页查询

        public IQueryable<T> GetPageEntities<S>(Expression<Func<T, bool>> lambda, Expression<Func<T, S>> orderLambda, int pageIndex, int pageSize, bool isAsc, out int totalCount)
        {
            var items = Context.Set<T>().Where(lambda);
            if (isAsc)
            {
                items = items.OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                items = items.OrderByDescending(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            totalCount = items.Count();
            return items;
        }

        #endregion

        

        #region 添加
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体</returns>
        public virtual T Insert(T model)
        {
            return Context.Set<T>().Add(model);
        }

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体的个数</returns>
        public virtual int Insert(IEnumerable<T> list)
        {
            if (list != null && list.Count() > 0)
            {
                foreach (T model in list)
                {
                    this.Insert(model);
                }
                return list.Count();
            }
            return 0;
        }

        #endregion

        #region 修改
        /// <summary>
        /// 更新一条记录(会查询数据库)
        /// </summary>
        /// <param name="entity">要修改的记录</param>
        /// <returns></returns>
        public virtual T Update(T entity)
        {
            DbEntityEntry<T> entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Context.Set<T>().Attach(entity);
                entry.State = EntityState.Modified;
            }
            return entity;
        }
        /// <summary>
        /// 使用一个新的对象来进行局部字段的更新 
        /// </summary>
        /// <param name="originalEntity">数据库中查询出来的数据</param>
        /// <param name="newEntity">新的对象，包含了要进行更新的字段的值</param>
        public void UpdateEntityFields(T originalEntity, T newEntity)
        {
            if (originalEntity != null && newEntity != null)
            {
                if (Context.Entry(originalEntity).State != EntityState.Unchanged)
                    Context.Entry(originalEntity).State = EntityState.Unchanged;
                Context.Entry(originalEntity).CurrentValues.SetValues(newEntity);
            }
        }
        /// <summary>
        /// 更新指定字段(这个)，不会先查询数据了
        /// </summary>
        /// <param name="entity">实体，一个新创建的实体</param>
        /// <param name="fileds">更新字段数组</param>
        public void UpdateEntityFields(T entity, IList<string> fields)
        {
            if (entity != null && fields != null)
            {
                Context.Set<T>().Attach(entity);
                var SetEntry = ((IObjectContextAdapter)Context).ObjectContext.
                    ObjectStateManager.GetObjectStateEntry(entity);
                foreach (var t in fields)
                {
                    SetEntry.SetModifiedProperty(t);
                }
            }
        }
        /// <summary>
        /// 更新指定字段，不会先查询数据了
        /// </summary>
        /// <param name="entity">实体，新创建的实体</param>
        /// <param name="fileds">更新字段数组</param>
        public void UpdateEntityFields(T entity, params string[] fields)
        {
            if (entity != null && fields != null)
            {
                Context.Set<T>().Attach(entity);

                var SetEntry = ((IObjectContextAdapter)Context).ObjectContext.
                    ObjectStateManager.GetObjectStateEntry(entity);
                foreach (var t in fields)
                {
                    SetEntry.SetModifiedProperty(t);
                }
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="model">要删除的记录</param>
        /// <returns></returns>
        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="lambdaWhere">lambda表达式</param>
        /// <returns></returns>
        public void Delete(Expression<Func<T, bool>> lambdaWhere)
        {

            var list = this.GetEntities(lambdaWhere);
            Context.Set<T>().RemoveRange(list);
        }
        #endregion

        #region 返回总数
        public long LongCount(Expression<Func<T, bool>> lambdaWhere)
        {
            return Context.Set<T>().LongCount(lambdaWhere);
        }
        public int Count(Expression<Func<T, bool>> lambdaWhere)
        {
            return Context.Set<T>().Count(lambdaWhere);
        }
        #endregion 
    }
}
