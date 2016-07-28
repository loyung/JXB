/*
 * 创建时间：2016-05-19
 * 创建人：刘自洋
 * 说明：新增对lamda的扩展方法
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Loyung.PublicMethod
{
   public static class LinqExtend
    {
        //使用方法：db.Profile.Where(BuildWhereInExpression<Profile,int>(v=>v.Id,ids);
       /// <summary>
       /// 创建可以使用SQL语句中IN的表达式
       /// </summary>
       /// <typeparam name="TElement">T类型可以传入具体类型</typeparam>
       /// <typeparam name="TValue">做IN字段的数据类型</typeparam>
       /// <param name="propertySelector">lamda表达式</param>
       /// <param name="values">所要In的集合</param>
       /// <returns></returns>
       private static  Expression<Func<TElement, bool>> BuildWhereInExpression<TElement, TValue>(Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
       {
           ParameterExpression p = propertySelector.Parameters.Single();
           if (!values.Any())
               return e => false;

           var equals = values.Select(value => (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
           var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

           return Expression.Lambda<Func<TElement, bool>>(body, p);
       }

       //使用方法：db.Profile.WhereIn(c => c.Id,new string[]{"1","2","3"});
       /// <summary>
       /// lamda表达式在where中使用IN
       /// </summary>
       /// <typeparam name="TElement"></typeparam>
       /// <typeparam name="TValue"></typeparam>
       /// <param name="source"></param>
       /// <param name="propertySelector"></param>
       /// <param name="values"></param>
       /// <returns>IQueryable对象结果</returns>
       public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
       {
           return source.Where(BuildWhereInExpression(propertySelector, values));
       }

       /// <summary>
       /// 去除元素重复项
       /// </summary>
       /// <typeparam name="TSource"></typeparam>
       /// <typeparam name="TKey"></typeparam>
       /// <param name="source"></param>
       /// <param name="keySelector"></param>
       /// <returns></returns>
       public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
       {
           HashSet<TKey> seenKeys = new HashSet<TKey>();
           foreach (TSource element in source)
           {
               if (seenKeys.Add(keySelector(element)))
               {
                   yield return element;
               }
           }
       }

     


    }
}
