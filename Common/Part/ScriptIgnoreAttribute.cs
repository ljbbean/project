
using System;

namespace Common.Part
{
    /// <summary>
    /// �������ֵ�����
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ScriptIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Ĭ��
        /// </summary>
        public ScriptIgnoreAttribute()
        {
        }
    }

    /// <summary>
    /// ʵ�������ԣ��ֶ���Сд��
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityClassAttribute : Attribute
    {
        private bool _lowerCaseKey = true;
        private string _tableName;

        /// <summary>
        /// 
        /// </summary>
        public EntityClassAttribute()
        {
        }

        /// <summary>
        /// �ֶλ������Ƿ�Сд
        /// </summary>
        public bool LowerCaseKey
        {
            get { return _lowerCaseKey; }
            set { _lowerCaseKey = value; }
        }


        /// <summary>
        /// ���ݱ���
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
    }
}
