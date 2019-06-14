using Hx.Framework.Dependency;
using HxBlogs.Model;
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
        /// 获取满足指定条件的一条数据（无跟踪查询）
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        IEnumerable<T> QueryEntitiesNoTrack(Expression<Func<T, bool>> lambdaWhere);


        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        Task<List<T>> QueryEntitiesAsync(Expression<Func<T, bool>> lambdaWhere);
        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        T QueryEntity(Expression<Func<T, bool>> lambdaWhere);
        /// <summary>
        /// 获取满足指定条件的一条数据(无跟踪查询)
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        T QueryEntityNoTrack(Expression<Func<T, bool>> lambdaWhere);


        /// <summary>
        /// 根据ID获取指定的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T QueryEntityByID(object id);
        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="condition">where条件</param>
        /// <returns></returns>
        T QueryEntityBySql(string condition);
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
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <param name="select">选择数据的条件表达式，可以用来选取指定的数据</param>
        /// <returns>满足当前条件的集合</returns>
        IEnumerable<TResult> QueryEntities<TResult>(Expression<Func<T, bool>> lambdaWhere, Expression<Func<T, TResult>> select);
        /// <summary>
        /// 获取满足指定条件的一条数据,无跟踪查询(查询出来的数据不可以修改，如果你做了修改，你会发现修改并不成功)
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <param name="select">选择数据的条件表达式，可以用来选取指定的数据</param>
        /// <returns>满足当前条件的实体集合</returns>
        IEnumerable<TResult> QueryEntitiesNoTrack<TResult>(Expression<Func<T, bool>> lambdaWhere, Expression<Func<T, TResult>> select);

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
        /// <summary>
        /// 使用一个新的对象来进行局部字段的更新 
        /// </summary>
        /// <param name="originalEntity">数据库中查询出来的数据</param>
        /// <param name="newEntity">新的对象，包含了要进行更新的字段的值</param>
        void UpdateEntityFields(T originalEntity, T newEntity);
        /// <summary>
        /// 更新指定字段(这个)，不会先查询数据了
        /// </summary>
        /// <param name="entity">实体，一个新创建的实体</param>
        /// <param name="fileds">更新字段数组</param>
        void UpdateEntityFields(T entity, List<string> fields);
        /// <summary>
        /// 更新指定字段，不会先查询数据了
        /// </summary>
        /// <param name="entity">实体，新创建的实体</param>
        /// <param name="fileds">更新字段数组</param>
        void UpdateEntityFields(T entity, params string[] fields);
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
        #region 返回数量
        /// <summary>
        /// 返回满足条件的数量
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        long LongCount(Expression<Func<T, bool>> lambdaWhere);
        /// <summary>
        /// 返回满足条件的数量
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        int  Count(Expression<Func<T, bool>> lambdaWhere);
        #endregion
    }
}
