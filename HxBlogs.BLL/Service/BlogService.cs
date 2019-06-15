using HxBlogs.IBLL;
using HxBlogs.Model;
using Hx.Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HxBlogs.IDAL;
using System.Linq.Expressions;

namespace HxBlogs.BLL
{
    public partial class BlogService : BaseService<Blog>, IBlogService
    {
        public override Blog BeforeInsert(Blog model)
        {
            model = base.BeforeInsert(model);
            if (model.IsPublish)
            {
                model.PublishDate = DateTime.Now;
            }
            return model;
        }
        public override Blog QueryEntity(Expression<Func<Blog, bool>> lambdaWhere, bool addcondition = true)
        {
            return base.QueryEntity(lambdaWhere, addcondition);
        }
        protected override Expression<Func<Blog, bool>> GetLambda(Expression<Func<Blog, bool>> lambdaWhere,bool addcondition)
        {
            if (addcondition)
            {
                ParameterExpression parameterExp = Expression.Parameter(typeof(Blog), "table");
                MemberExpression publishProp = Expression.Property(parameterExp, "Publish"),
                     deleteProp = Expression.Property(parameterExp, "Delete"),
                     privateProp = Expression.Property(parameterExp, "Private");
                BinaryExpression publishBin = Expression.Equal(publishProp, Expression.Constant("Y"));
                BinaryExpression deleteBin = Expression.Equal(deleteProp, Expression.Constant("N"));
                BinaryExpression privateBin = Expression.Equal(privateProp, Expression.Constant("N"));
                var bin = Expression.AndAlso(Expression.AndAlso(publishBin, deleteBin), privateBin);
                var lambda = Expression.Lambda<Func<Blog, bool>>(bin, parameterExp);
                return lambdaWhere.And(lambda);
            }
            return lambdaWhere;
        }
    }
}
