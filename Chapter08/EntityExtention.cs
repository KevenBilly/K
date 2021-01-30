using System;
using System.Data.Entity.Core.Objects.DataClasses;

namespace Chapter08
{
    public static class EntityExtention
    {
        /// <summary>
        /// 将EntityObject的基元属性进行复制，一般是用于解决不同Context下，但其实是相同对象的同步数据问题，比较少见
        /// </summary>
        /// <param name="originalObject">源对象</param>
        /// <param name="targetObject">目标对象</param>
        /// <returns></returns>
        /// <remarks>
        /// Added by Dahai - 2021/01/21 
        /// </remarks>
        public static bool CopyEdmScalarProperty<T>(this T originalObject, T targetObject) where T : EntityObject, new()
        {
            try
            {
                // 1.获取类型的基元属性
                var edmScalarProperties = Utils.GetPropertyInfosByAttribute<T, EdmScalarPropertyAttribute>();

                foreach (var edmScalarProperty in edmScalarProperties)
                {
                    // 2.1 获取源值
                    var orignalValue = edmScalarProperty.GetValue(originalObject, null);
                    // 2.2 将源值赋值到目标对象对应的属性上
                    edmScalarProperty.SetValue(targetObject, orignalValue, null);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
