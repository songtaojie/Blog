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
            if (Hx.Common.Helper.Helper.IsYes(model.IsPublish))
            {
                model.PublishDate = DateTime.Now;
            }
            return model;
        }
        protected override Expression<Func<Blog, bool>> GetLambda(Expression<Func<Blog, bool>> lambdaWhere)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(Blog), "b");
            MemberExpression publishProp = Expression.Property(parameterExp, "IsPublish"),
                 deleteProp = Expression.Property(parameterExp, "IsDeleted"),
                 privateProp = Expression.Property(parameterExp, "IsPrivate");
            BinaryExpression publishExp = Expression.Equal(publishProp, Expression.Constant("Y"));
            BinaryExpression deleteExp = Expression.Equal(deleteProp, Expression.Constant("N"));
            BinaryExpression privateExp = Expression.Equal(privateProp, Expression.Constant("N"));
            var bin = Expression.AndAlso(Expression.AndAlso(publishExp, deleteExp), privateExp);
            var lambda = Expression.Lambda<Func<Blog, bool>>(bin, parameterExp);
            return lambdaWhere.And(lambda);
        }
    }
}
