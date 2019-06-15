using HxBlogs.IDAL;
using HxBlogs.Model.Context;
using Hx.Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HxBlogs.Model;

namespace HxBlogs.BLL
{
    public abstract class BaseService<T> where T : Model.BaseModel, new()
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

        #region 查询单条数据
       
        public virtual T GetEntity(Expression<Func<T, bool>> lambda, bool defaultFilter = true)
        {
            lambda = GetLambda(lambda, defaultFilter);
            return this.baseDal.GetEntity(lambda);
        }
        public T GetEntityByID(object id, bool defaultFilter = true)
        {
            T model = this.baseDal.GetEntityByID(id);
            if (model != null && defaultFilter && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                if (string.Format("{0}", model["Delete"]) == "Y")
                {
                    model = null;
                }
            }
            return model;
        }
        public T GetEntityBySql(string condition)
        {
            return this.baseDal.GetEntityBySql(condition);
        }

        #endregion

        #region 

        #region 查询出集合数据
        public virtual IQueryable<T> GetEntities(Expression<Func<T, bool>> lambda, bool defaultFilter = true)
        {
            lambda = GetLambda(lambda, defaultFilter);
            return this.baseDal.GetEntities(lambda);
        }
        public virtual IQueryable<T> GetEntitiesNoTrack(Expression<Func<T, bool>> lambda,bool defaultFilter = true)
        {
            lambda = GetLambda(lambda, defaultFilter);
            return this.baseDal.GetEntitiesNoTrack(lambda);
        }
        #endregion

        public virtual bool Exist(object id,bool defaultFilter = true)
        {
            T model = this.GetEntityByID(id, defaultFilter);
            return model !=null;
        }
        public virtual bool Exist(Expression<Func<T, bool>> lambda, bool defaultFilter = true)
        {
            T model = this.GetEntity(lambda, defaultFilter);
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
                model["Delete"] = "Y";
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
            lambdaWhere = GetLambda(lambdaWhere, addcondition);
            return this.baseDal.LongCount(lambdaWhere);
        }
        /// <summary>
        /// 返回满足条件的数量
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> lambdaWhere, bool addcondition = true)
        {
            lambdaWhere = GetLambda(lambdaWhere, addcondition);
            return this.baseDal.Count(lambdaWhere);
        }
        #endregion

        #region 获取新的lambda
        protected virtual Expression<Func<T,bool>> GetLambda(Expression<Func<T, bool>> lambdaWhere,bool defaultFilter)
        {
            if (defaultFilter && typeof(Model.BaseEntity).IsAssignableFrom(typeof(T)))
            {
                ParameterExpression parameterExp = Expression.Parameter(typeof(T), "table");
                MemberExpression deleteProp = Expression.Property(parameterExp, "Delete");
                var lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(deleteProp, Expression.Constant("N")), parameterExp);
                return lambdaWhere.And(lambda);
            }
            return lambdaWhere;
        }
        #endregion
    }
}
