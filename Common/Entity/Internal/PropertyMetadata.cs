
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Entity.Internal
{
    internal sealed class PropertyMetadata
    {
        public string FieldName { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public FieldInfo FieldInfo { get; set; }

        public bool IsDbIgnore { get; set; }
    }
}
