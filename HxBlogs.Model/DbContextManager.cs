using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    public class DbContextManager
    {
        private DbContext Context
        {
            get { return DbFactory.GetDbSession().DbContext; }
        }
        #region 查询
        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <typeparam name="T">查询的模型类型</typeparam>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual T QueryEntity<T>(Expression<Func<T, bool>> lambdaWhere) where T : class
        {
            var result = Context.Set<T>().Where(lambdaWhere);
            if (result != null && result.Count() > 0)
            {
                return result.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 根据ID获取指定的数据
        /// </summary>
        /// <typeparam name="T">查询的模型类型</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T QueryEntityByID<T>(object id) where T : class
        {
            return Context.Set<T>().Find(id);
        }

        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <typeparam name="T">查询的模型类型</typeparam>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual IEnumerable<T> QueryEntities<T>(Expression<Func<T, bool>> lambdaWhere) 
            where T : class
        {
            var result = Context.Set<T>().Where(lambdaWhere);
            return result;
        }

        /// <summary>
        /// 分页形式的数据获取
        /// </summary>
        /// <typeparam name="T">查询的模型类型</typeparam>
        /// <typeparam name="S">在isAsc为false时，指定按什么类型的字段排序</typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示多少条数据</param>
        /// <param name="totalCount">输出参数，输出总共的条数，为了在页面分页栏显示</param>
        /// <param name="isAsc">true升序排序，false降序排序，false时需给出排序的lambda表达式</param>
        /// <param name="orderLambdaWhere">排序的lambda表达式</param>
        /// <param name="lambdaWhere">获取数据的lambda</param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageEntities<T,S>(int pageIndex, int pageSize, out int totalCount, bool isAsc, Expression<Func<T, S>> orderLambdaWhere, Expression<Func<T, bool>> lambdaWhere)
            where T: class
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
        /// 添加一条记录，只是标记为了添加，需要手动调用SaveChange
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体</returns>
        public T Add<T>(T model)where T:class
        {
            return Context.Set<T>().Add(model);
        }

        /// <summary>
        /// 批量添加记录，只是标记为了添加，需要手动调用SaveChange
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体的个数</returns>
        public int Add<T>(IEnumerable<T> list) where T : class
        {
            if (list != null && list.Count() > 0)
            {
                foreach (T model in list)
                {
                    this.Add(model);
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
        public virtual T Update<T>(T model) where T : class
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
        public virtual void Delete<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="lambdaWhere">lambda表达式</param>
        /// <returns></returns>
        public void Delete<T>(Expression<Func<T, bool>> lambdaWhere) where T : class
        {
            var list = this.QueryEntities<T>(lambdaWhere);
            if (list != null && list.Count() > 0)
            {
                Context.Set<T>().RemoveRange(list);
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存所有更改
        /// </summary>
        /// <returns></returns>
        public bool SaveChages()
        {
            return Context.SaveChanges() > 0;
        }
        #endregion
    }
}
