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
        public override Dictionary<string, IParameter> GetParameters(Dictionary<string, IParameter> parameters)
        {
            parameters = base.GetParameters(parameters);
            parameters.Add("Publish", new SqlParameter() { ParamValue = "Y" });
            parameters.Add("Private", new SqlParameter() { ParamValue = "N" });
            return parameters;
        }
        protected override Expression<Func<Blog, bool>> GetLambda(Expression<Func<Blog, bool>> lambdaWhere)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(Blog), "b");
            MemberExpression publishProp = Expression.Property(parameterExp, "Publish"),
                 deleteProp = Expression.Property(parameterExp, "Delete"),
                 privateProp = Expression.Property(parameterExp, "Private");
            ConstantExpression constN = Expression.Constant("N"),
                   constY = Expression.Constant("Y");
            BinaryExpression publishBin =  Expression.Equal(publishProp, constY),
               deleteBin =  Expression.Equal(deleteProp, constN),
               privateBin = Expression.Equal(privateProp, constN);
            var bin = Expression.AndAlso(Expression.AndAlso(publishBin, deleteBin), privateBin);
            var lambda = Expression.Lambda<Func<Blog, bool>>(bin, parameterExp);
            return lambdaWhere.And(lambda);
        }
    }
}
