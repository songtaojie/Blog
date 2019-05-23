using HxBlogs.IDAL;
using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.BLL
{
    public abstract class BaseService<T> where T: Model.BaseModel, new()
    {
        private static IDbSession _dbSession;
        protected internal IBaseDal<T> baseDal;
        //public BaseService(IBaseDal<T> baseDal)
        //{
        //    this.baseDal = baseDal;
        //}
        protected internal IDbSession DbSession
        {
            get {
                if (_dbSession == null)
                {
                    _dbSession = DbFactory.GetDbSession();
                }
                return _dbSession;
            }
        }

        #region 查询
        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <param name="excludeDeleted">排除已删除的,即只查询出未被删除的</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual T QueryEntity(Expression<Func<T, bool>> lambdaWhere,bool excludeDeleted = true)
        {
            if (excludeDeleted && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                var param = Expression.Property(Expression.Constant(new T()), "IsDeleted");
                var bin = Expression.Equal(param, Expression.Constant("N"));
                var lambda = Expression.Lambda<Func<T, bool>>
                      (Expression.AndAlso(bin,lambdaWhere.Body), lambdaWhere.Parameters);
                return this.baseDal.QueryEntity(lambda);
            }
            
            return this.baseDal.QueryEntity(lambdaWhere);
        }

        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <param name="excludeDeleted">排除已删除的,即只查询出未被删除的</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual IEnumerable<T> QueryEntities(Expression<Func<T, bool>> lambdaWhere, bool excludeDeleted = true)
        {
            if (excludeDeleted && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                var param = Expression.Property(Expression.Constant(new T()), "IsDeleted");
                var bin = Expression.Equal(param, Expression.Constant("N"));
                var lambda = Expression.Lambda<Func<T, bool>>
                      (Expression.AndAlso(bin,lambdaWhere.Body), lambdaWhere.Parameters);
                return this.baseDal.QueryEntities(lambda);
            }
            return this.baseDal.QueryEntities(lambdaWhere);
        }

        /// <summary>
        /// 根据ID获取指定的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T QueryEntityByID(object id, bool excludeDeleted = true)
        {
            T model = this.baseDal.QueryEntityByID(id);
            if (model != null && excludeDeleted && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                if (string.Format("{0}", model["IsDeleted"]) == "Y")
                {
                    model = null;
                }
            }
            return model;
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
        /// <param name="excludeDeleted">排除已删除的,即只查询出未被删除的</param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageEntities<S>(int pageIndex, int pageSize, out int totalCount, bool isAsc, Expression<Func<T, S>> orderLambdaWhere, Expression<Func<T, bool>> lambdaWhere,bool excludeDeleted = true)
        {
            if (excludeDeleted && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                var param = Expression.Property(Expression.Constant(new T()), "IsDeleted");
                var bin = Expression.Equal(param, Expression.Constant("N"));
                var lambda = Expression.Lambda<Func<T, bool>>
                      (Expression.AndAlso(bin, lambdaWhere.Body), lambdaWhere.Parameters);
                this.baseDal.QueryPageEntities(pageIndex, pageSize, out totalCount, isAsc, orderLambdaWhere, lambda);
            }
            return this.baseDal.QueryPageEntities(pageIndex, pageSize, out totalCount, isAsc, orderLambdaWhere, lambdaWhere);
        }

        /// <summary>
        /// 根据记录的ID判断数据库中是否存在某条记录
        /// </summary>
        /// <param name="id">记录的ID</param>
        /// <returns>true代表存在;false代表不存在</returns>
        public virtual bool Exist(object id,bool excludeDeleted = true)
        {
            T model = this.QueryEntityByID(id, excludeDeleted);
            return model !=null;
        }

        /// <summary>
        /// 根据表达式来判断是否存在某条记录
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns>true代表存在;false代表不存在</returns>
        public virtual bool Exist(Expression<Func<T, bool>> lambdaWhere, bool excludeDeleted = true)
        {
            T model = this.QueryEntity(lambdaWhere, excludeDeleted);
            return model != null;
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
            model = this.BeforeInsert(model);
            T m = this.baseDal.Insert(model);
            this.DbSession.SaveChange();
            return m;
        }
        /// <summary>
        /// 在添加数据之前
        /// </summary>
        /// <param name="model">要添加的模型数据</param>
        public virtual T BeforeInsert(T model)
        {

            return model;
        }
        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <param name="model">要添加的实体类</param>
        /// <returns>返回添加的实体的个数</returns>
        public virtual int Insert(IEnumerable<T> list)
        {
            return this.baseDal.Insert(list);
        }

        #endregion

        #region 修改
        public virtual T BeforeUpdate(T model)
        {
            if (model != null)
            {
                model["LastModifyTime"] = DateTime.Now;
            }
            return model;
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="model">要修改的记录</param>
        /// <returns></returns>
        public virtual T Update(T model)
        {
            model = this.BeforeUpdate(model);
            T m = this.baseDal.Update(model);
            return model;
        }
        #endregion

        #region 删除
        public virtual T BeforeLogicDelete(T model)
        {
            if (model != null)
            {
                model["DeleteTime"] = DateTime.Now;
                model["IsDelete"] = "Y";
            }
            return model;
        }
        /// <summary>
        /// 彻底删除一条记录
        /// </summary>
        /// <param name="model">要删除的记录</param>
        /// <returns></returns>
        public virtual void PhysicalDelete(T entity)
        {
            this.baseDal.Delete(entity);
        }

        /// <summary>
        /// 根据条件彻底删除记录
        /// </summary>
        /// <param name="lambdaWhere">lambda表达式</param>
        /// <returns></returns>
        public void PhysicalDelete(Expression<Func<T, bool>> lambdaWhere)
        {
            this.baseDal.Delete(lambdaWhere);
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="entity"></param>
        public void LogicDelete(T entity)
        {
            entity = BeforeLogicDelete(entity);
            this.Update(entity);
        }
        #endregion

        #region 返回数量
        /// <summary>
        /// 返回满足条件的数量
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public long LongCount(Expression<Func<T, bool>> lambdaWhere, bool excludeDeleted = true)
        {
            if (excludeDeleted && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                var param = Expression.Property(Expression.Constant(new T()), "IsDeleted");
                var bin = Expression.Equal(param, Expression.Constant("N"));
                var lambda = Expression.Lambda<Func<T, bool>>
                      (Expression.AndAlso(bin, lambdaWhere.Body), lambdaWhere.Parameters);
                return this.baseDal.LongCount(lambda);
            }
            return this.baseDal.LongCount(lambdaWhere);
        }
        /// <summary>
        /// 返回满足条件的数量
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> lambdaWhere, bool excludeDeleted = true)
        {
            if (excludeDeleted && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                var param = Expression.Property(Expression.Constant(new T()), "IsDeleted");
                var bin = Expression.Equal(param, Expression.Constant("N"));
                var lambda = Expression.Lambda<Func<T, bool>>
                      (Expression.AndAlso(bin, lambdaWhere.Body), lambdaWhere.Parameters);
                return this.baseDal.Count(lambda);
            }
            return this.baseDal.Count(lambdaWhere);
        }
        #endregion
    }
}
