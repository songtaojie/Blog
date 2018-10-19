using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                return result.FirstOrDefault();
            }
            return null;
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
        /// 分页形式的数据获取
        /// </summary>
        /// <typeparam name="S">在isAsc为false时，指定按什么类型的字段排序</typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示多少条数据</param>
        /// <param name="totalCount">输出参数，输出总共的条数，为了在页面分页栏显示</param>
        /// <param name="isAsc">true升序排序，false降序排序，false时需给出排序的lambda表达式</param>
        /// <param name="oederLambdaWhere">排序的lambda表达式</param>
        /// <param name="lambdaWhere">获取数据的lambda</param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageEntities<S>(int pageIndex, int pageSize, out int totalCount, bool isAsc, Expression<Func<T, S>> oederLambdaWhere, Expression<Func<T, bool>> lambdaWhere)
        {
            var items = Context.Set<T>().Where(lambdaWhere);
            if (isAsc)
            {
                items = items.OrderBy(oederLambdaWhere).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                items = items.OrderByDescending(oederLambdaWhere).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            totalCount = items.Count();
            return items;
        }

        /// <summary>
        /// 根据记录的ID判断数据库中是否存在某条记录
        /// </summary>
        /// <param name="id">记录的ID</param>
        /// <returns>true代表存在;false代表不存在</returns>
        public virtual bool Exists(object id)
        {
            T model = Context.Set<T>().Find(id);
            if (model != null)
                return true;
            return false;
        }

        /// <summary>
        /// 根据表达式来判断是否存在某条记录
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns>true代表存在;false代表不存在</returns>
        public virtual bool Exist(Expression<Func<T, bool>> lambdaWhere)
        {
            T model = this.QueryEntity(lambdaWhere);
            return model == null;
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
        /// 更新一条记录
        /// </summary>
        /// <param name="model">要修改的记录</param>
        /// <returns></returns>
        public virtual T Update(T model)
        {
            Context.Entry<T>(model).State = EntityState.Modified;
            return model;
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
        void Delete(Expression<Func<T, bool>> lambdaWhere)
        {
            var list = this.QueryEntities(lambdaWhere);
            if (list != null && list.Count() > 0)
            {
                Context.Set<T>().RemoveRange(list);
            }
        }
        #endregion
    }
}
