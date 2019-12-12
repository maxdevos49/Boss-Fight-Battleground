using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BFB.Engine.Helpers
{
    public static class LambdaExtensions
    {
        public static void SetPropertyValue<TModel, TValue>(this TModel target, Expression<Func<TModel, TValue>> memberLambda, TValue value)
        {
            if (!(memberLambda.Body is MemberExpression memberSelectorExpression)) return;
            
            PropertyInfo property = memberSelectorExpression.Member as PropertyInfo;
            
            if (property != null)
            {
                property.SetValue(target, value, null);
            }
        }
    }
}