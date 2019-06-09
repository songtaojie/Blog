using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
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
        #region 查询

        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual T QueryEntity(Expression<Func<T, bool>> lambdaWhere)
        {
            var result = Context.Set<T>().Where(lambdaWhere);
            if (result != null && result.Count() > 0)
            {
                return result.SingleOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 根据ID获取指定的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T QueryEntityByID(object id)
        {
            return Context.Set<T>().Find(id);
        }

        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual IEnumerable<T> QueryEntities(Expression<Func<T, bool>> lambdaWhere)
        {
            var result = Context.Set<T>().Where(lambdaWhere);
            return result;
        }
        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual async Task<List<T>> QueryEntitiesAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            var result = await Context.Set<T>().Where(lambdaWhere).ToListAsync();
            return result;
        }

        /// <summary>
        /// 分页形式的数据获取
        /// </summary>
        /// <typeparam name="S">在isAsc为false时，指定按什么类型的字段排序</typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示多少条数据</param>
        /// <param name="totalCount">输出参数，输出总共的条数，为了在页面分页栏显示</param>
        /// <param name="isAsc">true升序排序，false降序排序，false时需给出排序的lambda表达式</param>
        /// <param name="orderLambdaWhere">排序的lambda表达式</param>
        /// <param name="lambdaWhere">获取数据的lambda</param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageEntities<S>(int pageIndex, int pageSize, out int totalCount, bool isAsc, Expression<Func<T, S>> orderLambdaWhere, Expression<Func<T, bool>> lambdaWhere)
        {
            var items = Context.Set<T>().Where(lambdaWhere);
            if (isAsc)
            {
                items = items.OrderBy(orderLambdaWhere).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                items = items.OrderByDescending(orderLambdaWhere).Skip((pageIndex - 1) * pageSize).Take(pageSize);
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
        public void UpdateEntityFields(T entity, List<string> fields)
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
            var list = this.QueryEntities(lambdaWhere);
            if (list != null && list.Count() > 0)
            {
                Context.Set<T>().RemoveRange(list);
            }
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
