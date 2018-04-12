using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoRequest
{
    public static class ObjectExt
    {
        public static bool IsEmptyObject(this object obj)
        {
            if (obj == null || obj.ToString().Length == 0)
            {
                return true;
            }
            return false;
        }
    }
}
