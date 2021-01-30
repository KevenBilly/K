using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;

namespace Chapter08
{
    public class Utils
    {
        /// <summary>
        /// 获取泛型TSource中使用了TAttribute标识的属性集合(默认地，属性集合的顺序就是泛型类TSource的属性“编码顺序”)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// Added by Dahai - 2020/12/10
        /// </remarks>
        public static List<System.Reflection.PropertyInfo> GetPropertyInfosByAttribute<TSource, TAttribute>() where TSource : new()
                                                                                                              where TAttribute : System.Attribute, new()
        {
            return new TSource().GetType().GetProperties().Where(property => property.IsDefined(typeof(TAttribute), false))
                                                          .ToList();
        }

        /// <summary>
        /// 获取泛型TSource中唯一一个使用了TAttribute的属性信息
        /// </summary>
        /// <typeparam name="TSource">泛型参数必须为class,毕竟要获取其内部的属性成员</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        /// <exception cref="">
        /// 1.如果该泛型TSource没有一个被TAttribute标识的属性，抛出异常
        /// 2.如果该泛型TSource有多个被TAttribute标识的数据，也抛出异常
        /// </exception>
        /// <remarks>
        /// Added by Dahai - 2020/12/10
        /// </remarks>
        public static System.Reflection.PropertyInfo GetOnlyOnePropertyInfoByAttribute<TSource, TAttribute>() where TSource : new() where TAttribute : System.Attribute, new()
        {
            var customizedAttributeProperties = GetPropertyInfosByAttribute<TSource, TAttribute>();
            // 1.1 判断是否没有被TAttribute标识的属性
            if (customizedAttributeProperties.Count == 0)
            {
                string msg = string.Format("泛型实参 {0} 中必须有且仅有一个被自定性特性 {1} 标识的属性成员", new TSource().ToString(), new TAttribute().ToString());
                throw new ObjectNotFoundException(msg);
            }
            // 1.2 判断是否多于1个属性成员被TAttribute标识
            if (customizedAttributeProperties.Count > 1)
            {
                string msg = string.Format("泛型实参 {0} 中必须有且仅有一个被自定性特性 {1} 标识的属性成员", new TSource().ToString(), new TAttribute().ToString());
                throw new Exception(msg);
            }
            // 2.此时可以直接返回First()或者FirstOrDefault,因为前面的验证已经确定当前被指定的自定义特性标识的属性有且仅有一个
            return customizedAttributeProperties.First();
        }
    }
}