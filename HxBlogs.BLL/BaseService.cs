using HxBlogs.IDAL;
using HxBlogs.Model.Context;
using Hx.Common.Extension;
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
        public virtual T QueryEntity(Expression<Func<T, bool>> lambdaWhere,bool addcondition = true)
        {
            if (addcondition && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return this.baseDal.QueryEntity(GetLambda(lambdaWhere));
            }
            
            return this.baseDal.QueryEntity(lambdaWhere);
        }
        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual async Task<List<T>> QueryEntitiesAsync(Expression<Func<T, bool>> lambdaWhere, bool addcondition = true)
        {
            if (addcondition && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return await this.baseDal.QueryEntitiesAsync(GetLambda(lambdaWhere));
            }
            return await this.baseDal.QueryEntitiesAsync(lambdaWhere);
        }
        /// <summary>
        /// 获取满足指定条件的一条数据
        /// </summary>
        /// <param name="lambdaWhere">获取数据的条件lambda</param>
        /// <param name="addcondition">排除已删除的,即只查询出未被删除的</param>
        /// <returns>满足当前条件的一个实体</returns>
        public virtual IEnumerable<T> QueryEntities(Expression<Func<T, bool>> lambdaWhere, bool addcondition = true)
        {
            if (addcondition && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return this.baseDal.QueryEntities(GetLambda(lambdaWhere));
            }
            return this.baseDal.QueryEntities(lambdaWhere);
        }

        /// <summary>
        /// 根据ID获取指定的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T QueryEntityByID(object id, bool addcondition = true)
        {
            T model = this.baseDal.QueryEntityByID(id);
            if (model != null && addcondition && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
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
        /// <param name="addcondition">是否添加额外的条件，如自动添加删除条件等</param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageEntities<S>(int pageIndex, int pageSize, out int totalCount, bool isAsc, Expression<Func<T, S>> orderLambdaWhere, Expression<Func<T, bool>> lambdaWhere,bool addcondition = true)
        {
            if (addcondition && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                this.baseDal.QueryPageEntities(pageIndex, pageSize, out totalCount, isAsc, orderLambdaWhere, GetLambda(lambdaWhere));
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
            int count = this.baseDal.Insert(list);
            this.DbSession.SaveChange();
            return count;
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
        /// 使用一个新的对象来进行局部字段的更新 
        /// </summary>
        /// <param name="originalEntity">数据库中查询出来的数据</param>
        /// <param name="newEntity">新的对象，包含了要进行更新的字段的值</param>
        public bool UpdateEntityFields(T originalEntity, T newEntity)
        {
            newEntity = BeforeUpdate(newEntity);
            this.baseDal.UpdateEntityFields(originalEntity, newEntity);
            return this.DbSession.SaveChange();
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">要修改的记录</param>
        /// <returns></returns>
        public virtual T Update(T entity)
        {
            entity = this.BeforeUpdate(entity);
            T m = this.baseDal.Update(entity);
            this.DbSession.SaveChange();
            return entity;
        }

        /// <summary>
        /// 更新指定字段(这个)，不会先查询数据了
        /// </summary>
        /// <param name="entity">实体，一个新创建的实体</param>
        /// <param name="fileds">更新字段数组</param>
        public bool UpdateEntityFields(T entity, List<string> fields)
        {
            if (fields != null)
            {
                fields.Add("LastModifyTime");
            }
            entity = this.BeforeUpdate(entity);
            this.baseDal.UpdateEntityFields(entity, fields);
            return this.DbSession.SaveChange();
        }
        /// <summary>
        /// 更新指定字段，不会先查询数据了
        /// </summary>
        /// <param name="entity">实体，新创建的实体</param>
        /// <param name="fileds">更新字段数组</param>
        public bool UpdateEntityFields(T entity, params string[] fields)
        {
            List<string> fieldList = null;
            if (fields != null)
            {
                fieldList =  fields.ToList();
                fieldList.Add("LastModifyTime");
            }
            entity = this.BeforeUpdate(entity);
            this.baseDal.UpdateEntityFields(entity, fieldList);
            return this.DbSession.SaveChange();
        }
        #endregion

        #region 删除
        public virtual T BeforeLogicDelete(T model)
        {
            if (model != null)
            {
                model["DeleteTime"] = DateTime.Now;
                model["IsDeleted"] = true;
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
            this.DbSession.SaveChange();
        }

        /// <summary>
        /// 根据条件彻底删除记录
        /// </summary>
        /// <param name="lambdaWhere">lambda表达式</param>
        /// <returns></returns>
        public void PhysicalDelete(Expression<Func<T, bool>> lambdaWhere)
        {
            this.baseDal.Delete(lambdaWhere);
            this.DbSession.SaveChange();
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
        public long LongCount(Expression<Func<T, bool>> lambdaWhere, bool addcondition = true)
        {
            if (addcondition && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return this.baseDal.LongCount(GetLambda(lambdaWhere));
            }
            return this.baseDal.LongCount(lambdaWhere);
        }
        /// <summary>
        /// 返回满足条件的数量
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> lambdaWhere, bool addcondition = true)
        {
            if (addcondition && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return this.baseDal.Count(GetLambda(lambdaWhere));
            }
            return this.baseDal.Count(lambdaWhere);
        }
        #endregion

        #region 获取新的lambda
        protected virtual Expression<Func<T,bool>> GetLambda(Expression<Func<T, bool>> lambdaWhere)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(T), "b");
            MemberExpression deleteProp = Expression.Property(parameterExp, "IsDeleted");
            var lambda = Expression.Lambda<Func<T, bool>>(Expression.Not(deleteProp), parameterExp);
            return lambdaWhere.And(lambda);
        }
        #endregion
    }
}
