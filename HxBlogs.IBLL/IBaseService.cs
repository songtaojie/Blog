using HxBlogs.Framework.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.IBLL
{
    public interface IBaseService<T>: ITransientDependency where T : class, new()
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
        /// 根据ID获取指定的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T QueryEntityByID(object id);

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
        IEnumerable<T> QueryPageEntities<S>(int pageIndex, int pageSize, out int totalCount, bool isAsc, Expression<Func<T, S>> oederLambdaWhere, Expression<Func<T, bool>> lambdaWhere);

        /// <summary>
        /// 根据记录的ID判断数据库中是否存在某条记录
        /// </summary>
        /// <param name="id">记录的ID</param>
        /// <returns>true代表存在;false代表不存在</returns>
        bool Exist(object id);

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
        /// <returns>返回添加后的实体对象</returns>
        T Insert(T model);

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体个数</returns>
        int Insert(IEnumerable<T> list);
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
        /// 彻底删除一条记录
        /// </summary>
        /// <param name="model">要删除的记录</param>
        /// <returns></returns>
        void PhysicalDelete(T model);

        /// <summary>
        /// 根据条件彻底删除一条记录
        /// </summary>
        /// <param name="lambdaWhere">lambda表达式</param>
        /// <returns></returns>
        void PhysicalDelete(Expression<Func<T, bool>> lambdaWhere);
        #endregion
    }
}
