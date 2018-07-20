using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Part
{
    /// <summary>
    /// 参数属性设置器
    /// </summary>
    public interface IParamPropertySetter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        void PropertySetterBeforeInvoke(string propertyName, object value);
    }
}
