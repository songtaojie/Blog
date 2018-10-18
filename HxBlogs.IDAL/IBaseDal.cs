using HxBlogs.Framework.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.IDAL
{
    public interface IBaseDal<T>:ITransientDependency 
        where T:class,new()
    {
        #region 查询
        /// <summary>
        /// 获取满足指定条件的所有数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>当前实体的集合</returns>
        IEnumerable<T> QueryEntities(Expression<Func<T, bool>> lambdaWhere);

        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        T QueryEntity(Expression<Func<T, bool>> lambdaWhere);

        /// <summary>
        /// 根据记录的ID判断数据库中是否存在某条记录
        /// </summary>
        /// <param name="id">记录的ID</param>
        /// <returns>true代表存在;false代表不存在</returns>
        bool Exists(object id);

        /// <summary>
        /// 根据表达式来判断是否存在某条记录
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns>true代表存在;false代表不存在</returns>
        bool Exist(Expression<Func<T, bool>> lambdaWhere);
        #endregion

        #region 添加
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体的ID</returns>
        object Add(T model);

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体的ID的集合</returns>
        List<object> Add(IEnumerable<T> list);
        #endregion

        #region 修改
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="model">要修改的记录</param>
        /// <returns></returns>
        T Update(T model);
        #endregion

        #region 删除
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="model">要删除的记录</param>
        /// <returns></returns>
        void Delete(T model);

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="lambdaWhere">lambda表达式</param>
        /// <returns></returns>
        void Delete(Expression<Func<T, bool>> lambdaWhere);
        #endregion
    }
}
